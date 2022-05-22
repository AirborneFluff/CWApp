using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using API.Extensions;

namespace API.Data
{
    public class Importer
    {

        static List<string> _existingPartCodes;
        static List<string> _existingSupplierNames;

        static List<Supplier> _cachedSuppliers;

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

        public static void GetPartFromLine(string RowData, IUnitOfWork unitOfWork)
        {
            NewPartDto newPart = new NewPartDto();
            RowData = Regex.Replace(RowData, @"(?<=\,)""|""(?=\,)|^""|""$", "");
            string[] fieldData = RowData.Split(',');
            if (fieldData.Length != 22) return;

            // Get partcode
            string _partCode = fieldData[(int)Fields.PartCode];
            if (PartCodeExists(_partCode)) return;

            _existingPartCodes.Add(_partCode);
            newPart.PartCode = _partCode;

            // Get description
            newPart.Description = fieldData[(int)Fields.Description];

            // Parse buffer
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
                //if (fieldData[(int)Fields.Buffer] != "")
                    //ErrorLog.Add($"\nBuffer maybe missing from \"{newPart.PartCode}\"\nUnable to parse buffer value");

                newPart.BufferValue = 0;
                newPart.BufferUnit = _bufferSuffix;
            }

            unitOfWork.PartsRepository.AddPart(newPart);
        }

        public static void GetSuppliersFromLine(string RowData, IUnitOfWork unitOfWork)
        {
            NewSupplierDto newSupplier = new NewSupplierDto();
            RowData = Regex.Replace(RowData, @"(?<=\,)""|""(?=\,)|^""|""$", "");
            string[] fieldData = RowData.Split(',');
            if (fieldData.Length != 22) return;

            // Parsing supplier names
            string _name1 = fieldData[(int)Fields.Supplier1];
            string _name2 = fieldData[(int)Fields.Supplier2];
            string _name3 = fieldData[(int)Fields.Supplier3];

            if (!string.IsNullOrEmpty(_name1) && !SupplierExists(_name1))
            {
                unitOfWork.SuppliersRepository.AddSupplier(new NewSupplierDto { Name = _name1 });
                _existingSupplierNames.Add(_name1.ToUpper());
            }
            if (!string.IsNullOrEmpty(_name2) && !SupplierExists(_name2))
            {
                unitOfWork.SuppliersRepository.AddSupplier(new NewSupplierDto { Name = _name2 });
                _existingSupplierNames.Add(_name2.ToUpper());
            }
            if (!string.IsNullOrEmpty(_name3) && !SupplierExists(_name3))
            {
                unitOfWork.SuppliersRepository.AddSupplier(new NewSupplierDto { Name = _name3 });
                _existingSupplierNames.Add(_name3.ToUpper());
            }
        }

        public static async void GetSupplySourcesFromLine(string RowData, IUnitOfWork unitOfWork)
        {
            RowData = Regex.Replace(RowData, @"(?<=\,)""|""(?=\,)|^""|""$", "");
            string[] fieldData = RowData.Split(',');
            if (fieldData.Length != 22) return;

            // Get Part
            Part part = await unitOfWork.PartsRepository.GetPartByPartCode(fieldData[(int)Fields.PartCode]);
            if (part == null) return;

            // Get Suppliers
            Supplier supplier1 = await unitOfWork.SuppliersRepository.GetSupplierByName(fieldData[(int)Fields.Supplier1]);
            Supplier supplier2 = await unitOfWork.SuppliersRepository.GetSupplierByName(fieldData[(int)Fields.Supplier2]);
            Supplier supplier3 = await unitOfWork.SuppliersRepository.GetSupplierByName(fieldData[(int)Fields.Supplier3]);

            SupplySource source1;
            SupplySource source2;
            SupplySource source3;

            if (supplier1 != null)
            {
                bool _RoHS = false;
                bool.TryParse(fieldData[(int)Fields.RoHS1], out _RoHS);

                source1 = new SupplySource();
                source1.Supplier = supplier1;
                source1.SupplierSKU = fieldData[(int)Fields.Code1];
                source1.Notes = "--Import--\n" + fieldData[(int)Fields.Price1];
                source1.RoHS = _RoHS;
                source1.PartId = part.Id;

                unitOfWork.SupplySourceRepository.AddSupplySource(source1);
            }
            
            if (supplier2 != null)
            {
                bool _RoHS = false;
                bool.TryParse(fieldData[(int)Fields.RoHS2], out _RoHS);

                source2 = new SupplySource();
                source2.Supplier = supplier2;
                source2.SupplierSKU = fieldData[(int)Fields.Code2];
                source2.Notes = "--Import--\n" + fieldData[(int)Fields.Price2];
                source2.RoHS = _RoHS;
                source2.PartId = part.Id;

                unitOfWork.SupplySourceRepository.AddSupplySource(source2);
            }
            
            if (supplier3 != null)
            {
                bool _RoHS = false;
                bool.TryParse(fieldData[(int)Fields.RoHS3], out _RoHS);

                source3 = new SupplySource();
                source3.Supplier = supplier3;
                source3.SupplierSKU = fieldData[(int)Fields.Code3];
                source3.Notes = "--Import--\n" + fieldData[(int)Fields.Price3];
                source3.RoHS = _RoHS;
                source3.PartId = part.Id;

                unitOfWork.SupplySourceRepository.AddSupplySource(source3);
            }
        }

        
        public static async void GetPricesFromLine(string RowData, IUnitOfWork unitOfWork)
        {
            RowData = Regex.Replace(RowData, @"(?<=\,)""|""(?=\,)|^""|""$", "");
            string[] fieldData = RowData.Split(',');
            if (fieldData.Length != 22) return;

            // Get Part
            Part part = await unitOfWork.PartsRepository.GetPartByPartCode(fieldData[(int)Fields.PartCode]);
            if (part == null) return;

            foreach (var source in part.SupplySources)
            {
                source.Prices = new List<SourcePrice>();

                // Pack Size
                Regex rx_packSize = new Regex(@"(?<!\=\s)(?<PackSize>\b\d+[kM]?)(?=\s?(?>PACK|PK|REEL|LENGTH|BX))", RegexOptions.IgnoreCase);
                Match rxC_packSize = rx_packSize.Match(source.Notes);
                string _packSize = rxC_packSize.Groups[1].Value;
                if (_packSize == "") source.PackSize = 1;
                else
                {
                    float packSize = 1;
                    float.TryParse(_packSize, out packSize);
                    source.PackSize = packSize;
                }

                // Minimum Order Quantity
                Regex rx_moq = new Regex(@"(?<=(?>MOQ|O\/M)\=)\s?(?<OM>\b\d+[kM]?)\s?(?>PACK[s]?|PK[s]?|BX[s]?)?", RegexOptions.IgnoreCase);
                Match rxC_moq = rx_moq.Match(source.Notes);
                string _moq = rxC_moq.Groups[1].Value;
                if (_moq == "") source.MinimumOrderQuantity = 1;
                else
                {
                    float moq = 1;
                    float.TryParse(_moq, out moq);
                    source.MinimumOrderQuantity = moq;
                }

                // Price Breaks
                Regex rx_quantityPrice = new Regex(@"(?<Quantity>\d+\.?\d*|E\/?C|E\/?C\s*?PRICE)\s*?[\+|\@|\=]\s+?(?<Price>\d+\.\d+)", RegexOptions.IgnoreCase);
                MatchCollection rxC_quantityPrice = rx_quantityPrice.Matches(source.Notes);
                float _quantityStore;
                float _priceStore;
                foreach (Match match in rxC_quantityPrice)
                {
                    if ((match.Groups[1].Value.Contains("E/C") || match.Groups[1].Value.Contains("EC"))
                        && float.TryParse(match.Groups[2].Value, out _priceStore))
                    {
                        source.Prices.Add(new SourcePrice { UnitPrice = _priceStore});
                    }
                    else if (float.TryParse(match.Groups[1].Value, out _quantityStore)
                        && float.TryParse(match.Groups[2].Value, out _priceStore))
                    {
                        source.Prices.Add(new SourcePrice {
                            UnitPrice = _priceStore,
                            Quantity = _quantityStore
                            });
                    }
                }
            }
        }

        static bool PartCodeExists(string partCode)
        {
            if (_existingPartCodes.Contains(partCode)) return true;
            return false;
        }
        static bool SupplierExists(string supplierName)
        {
            if (_existingSupplierNames.Contains(supplierName.ToUpper())) return true;
            return false;
        }

        public async static Task Begin(IUnitOfWork unitOfWork)
        {
            _existingPartCodes = new List<string>();
            _existingSupplierNames = new List<string>();
            
            string text = await File.ReadAllTextAsync("Data/table.txt");
            string[] lines = text.Split('\n');
            int total = lines.Length;

            foreach (var line in lines)
                GetPartFromLine(line, unitOfWork);
            await unitOfWork.Complete();

            foreach (var line in lines)
                GetSuppliersFromLine(line, unitOfWork);
            await unitOfWork.Complete();
            
            foreach (var line in lines)
                GetSupplySourcesFromLine(line, unitOfWork);
            await unitOfWork.Complete();

            foreach (var line in lines)
                GetPricesFromLine(line, unitOfWork);
            await unitOfWork.Complete();
        }
    }
}