using System;
using System.Diagnostics.CodeAnalysis;

namespace TestWebApi.Models
{

    public abstract class BaseEntity
    {
        public virtual Guid Id { get; set; }

    }
}