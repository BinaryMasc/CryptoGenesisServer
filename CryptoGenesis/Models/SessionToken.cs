//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CryptoGenesis.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SessionToken
    {
        public int SessionTokenId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string TokenString { get; set; }
        public System.DateTime CreationDate { get; set; }
    
        public virtual User User { get; set; }
    }
}