declare global {  
  interface String {  
   isNullOrWhiteSpace(): Boolean;  
  }  
 }  
 String.prototype.isNullOrWhiteSpace = function(): Boolean {  
   return !String(this) || !String(this).trim();
 }  
 export {}; 