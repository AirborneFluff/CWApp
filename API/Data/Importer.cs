using System.Text.RegularExpressions;
using API.Extensions;

namespace API.Data
{
    public class Importer
    {
        static string _noneParsableLines;
        static List<string> _existingPartCodes;
        static List<Part> _parts;
        static List<string> _errorLog;

        enum Fields
        {
            ID,
            PartCode,
            Description,
            Supplier1,
            Code1,
            RoHS1,
            Price1,
            Supplier2,
            Code2,
            Price2,
            RoHS2,
            Supplier3,
            Code3,
            RoHS3,
            Price3,
            CMRT,
            Date,
            TypicalQuantity,
            TypicalPrice,
            Buffer,
            Job18407,
            Job18406
        }
        enum PatternState
        {
            None,
            SearchingForQuantity,
            SearchingForPrice,
        }

        public static async void ConvertLine(string RowData, List<string> ErrorLog, IUnitOfWork unitOfWork)
        {
            NewPartDto newPart = new NewPartDto();
            RowData = Regex.Replace(RowData, @"(?<=\,)""|""(?=\,)|^""|""$", "");
            string[] fieldData = RowData.Split(',');
            if (fieldData.Length != 22)
            {
                ErrorLog.Add($"\nCannot convert \"{RowData}\"\nData doesn't contain the right number of collumns: Line has -> {fieldData.Length} collums");
                return;
            }

            #region PartCode
            string _partCode = fieldData[(int)Fields.PartCode];
            if (!PartCodeExists(_partCode))
                newPart.PartCode = _partCode.ToString();
            else
            {
                ErrorLog.Add($"\nERROR! Cannot convert {RowData}\nCannot parse 'PartCode'");
                return;
            }
            #endregion

            #region Description
            newPart.Description = fieldData[(int)Fields.Description];
            #endregion

            #region Buffer
            float _bufferValue;
            float _bufferMagnitude = 1;
            string _bufferValText;
            string _bufferSuffix;

            RegexOptions options = RegexOptions.IgnoreCase;
            Regex rx_buffer = new Regex(@"(?<Values>^\d+\.?\d*)(?<Magnitude>k?)(?>\s*?(?<Suffix>\S+.+))?", options);
            Match rxC_buffer = rx_buffer.Match(fieldData[(int)Fields.Buffer]);

            _bufferValText = rxC_buffer.Groups[1].Value;
            if (rxC_buffer.Groups[2].Value == "k" || rxC_buffer.Groups[2].Value == "K")
                _bufferMagnitude = 1000;
            _bufferSuffix = rxC_buffer.Groups[3].Value;

            if (float.TryParse(_bufferValText, out _bufferValue))
            {
                newPart.BufferValue = _bufferValue * _bufferMagnitude;
                newPart.BufferUnit = _bufferSuffix;
            }
            else
            {
                if (fieldData[(int)Fields.Buffer] != "")
                    ErrorLog.Add($"\nBuffer maybe missing from \"{newPart.PartCode}\"\nUnable to parse buffer value");

                newPart.BufferValue = 0;
                newPart.BufferUnit = _bufferSuffix;
            }
            #endregion

            unitOfWork.PartsRepository.AddPart(newPart);
            await unitOfWork.Complete();
            var part = await unitOfWork.PartsRepository.GetPartByPartCode(_partCode);

            #region Supply Sources

            // Parsing supplier names
            string _name1 = fieldData[(int)Fields.Supplier1];
            string _name2 = fieldData[(int)Fields.Supplier2];
            string _name3 = fieldData[(int)Fields.Supplier3];

            List<int> _sourceFieldIndexs = new List<int>();
            if (!string.IsNullOrEmpty(_name1))
            {
                var supplier = await unitOfWork.SuppliersRepository.GetSupplierByName(_name1);
                if (supplier == null)
                {
                    unitOfWork.SuppliersRepository.AddSupplier(new NewSupplierDto{ Name = _name1 });
                    await unitOfWork.Complete();
                    supplier = await unitOfWork.SuppliersRepository.GetSupplierByName(_name1);
                }

                bool _RoHS = false;
                bool.TryParse(fieldData[(int)Fields.RoHS1], out _RoHS);

                var supplySource = new SupplySource
                {
                    Supplier = supplier,
                    SupplierSKU = fieldData[(int)Fields.Code1],
                    Notes = "--Import--\n" + fieldData[_sourceFieldIndexs[0]],
                    RoHS = _RoHS
                };
                part.SupplySources.Add(supplySource);
                _sourceFieldIndexs.Add(6);
            }
            #endregion

            /*
            #region Source Data
            int i = 0;
            foreach (var source in _supplySource)
            {
                source.PriceBreaks = new List<SourcePrice>();

                // Pack Size
                Regex rx_packSize = new Regex(@"(?<!\=\s)(?<PackSize>\b\d+[kM]?)(?=\s?(?>PACK|PK|REEL|LENGTH|BX))", RegexOptions.IgnoreCase);
                Match rxC_packSize = rx_packSize.Match(fieldData[_sourceFieldIndexs[i]]);
                string _packSize = rxC_packSize.Groups[1].Value;
                if (_packSize == "") source.PackSize = "1";
                else source.PackSize = _packSize;

                // Minimum Order Quantity
                Regex rx_moq = new Regex(@"(?<=(?>MOQ|O\/M)\=)\s?(?<OM>\b\d+[kM]?)\s?(?>PACK[s]?|PK[s]?|BX[s]?)?", RegexOptions.IgnoreCase);
                Match rxC_moq = rx_moq.Match(fieldData[_sourceFieldIndexs[i]]);
                string _moq = rxC_moq.Groups[1].Value;
                if (_moq == "") source.MinimumOrder = "1";
                else source.MinimumOrder = _moq;

                // Price Breaks
                Regex rx_quantityPrice = new Regex(@"(?<Quantity>\d+\.?\d*|E\/?C|E\/?C\s*?PRICE)\s*?[\+|\@|\=]\s+?(?<Price>\d+\.\d+)", RegexOptions.IgnoreCase);
                MatchCollection rxC_quantityPrice = rx_quantityPrice.Matches(fieldData[_sourceFieldIndexs[i]]);
                double _quantityStore;
                double _priceStore;
                foreach (Match match in rxC_quantityPrice)
                {
                    if ((match.Groups[1].Value.Contains("E/C") || match.Groups[1].Value.Contains("EC"))
                        && double.TryParse(match.Groups[2].Value, out _priceStore))
                    {
                        source.PriceBreaks.Add(new SourcePrice(_priceStore, 1));
                    }
                    else if (double.TryParse(match.Groups[1].Value, out _quantityStore)
                        && double.TryParse(match.Groups[2].Value, out _priceStore))
                    {
                        source.PriceBreaks.Add(new SourcePrice(_priceStore, _quantityStore));
                    }
                }

                if (source.PriceBreaks.Count() == 0)
                {
                    _noneParsableLines += $"{newPart.PartCode} - {source.SupplierName} - {Enum.GetName(typeof(Fields), _sourceFieldIndexs[i])}\n" + fieldData[_sourceFieldIndexs[i]] + "\n\n";
                }

                i++;
            }
            newPart.SupplySources = _supplySource;
            #endregion
            */
            
        }
        static bool PartCodeExists(string partCode)
        {
            if (_existingPartCodes.Contains(partCode)) return true;
            else return false;
        }

        public async static Task Begin(IUnitOfWork unitOfWork)
        {
            _existingPartCodes = await unitOfWork.PartsRepository.GetAllPartCodes();

            if (_existingPartCodes == null) _existingPartCodes = new List<string>();

            _errorLog = new List<string>();
            
            string text = await File.ReadAllTextAsync("Data/table.txt");
            string[] lines = text.Split('\n');
            int total = lines.Length;

            foreach (var line in lines)
            {
                ConvertLine(line, _errorLog, unitOfWork);
            }

            await unitOfWork.Complete();
        }
    }
}