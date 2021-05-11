using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BatchAPI.Model
{
    public class Batch
    {
        [Key]
        [JsonIgnore]
        public Guid BatchId { get; set; }
        public string BusinessUnit { get; set; }

        [JsonIgnore]
        [ForeignKey("ACLID")]
        [NotMapped]
        public int AId { get; set; }

        public virtual ACL ACL { get; set; }

        public List<Attributes> Attributes { get; set; }
        public DateTime ExpiryDate { get; set; }

        public class BatchValidator : AbstractValidator<Batch>
        {
            public BatchValidator()
            {
                RuleFor(p => p.BusinessUnit).NotNull().WithMessage("Bad Request - there are one or more errors in the specified parameters");
                RuleFor(p => p.BusinessUnit).NotEmpty().WithMessage("Bad Request - there are one or more errors in the specified parameters");
                RuleFor(p => p.BatchId.ToString()).Must(ValidateGUID).WithErrorCode("Not a guid");

                RuleFor(f => f.Attributes).Must((f, d) =>
                {
                    for (int i = 0; i < d.Count; i++)
                    {
                        if (String.IsNullOrEmpty(d[i].Key))
                            return false;
                    }

                    return true;
                })
                .WithMessage("Attribute key(s) cannot be empty.");

                RuleFor(f => f.Attributes).Must((f, d) =>
                {
                    for (int i = 0; i < d.Count; i++)
                    {
                        if (String.IsNullOrEmpty(d[i].Value))
                            return false;
                    }

                    return true;
                })
                .WithMessage("Attribute value(s) cannot be empty.");

            }

            public bool ValidateGUID(string value)
            {
                bool GuidFormat = false;

                GuidFormat = Guid.TryParse(value, out var result) || result == Guid.Empty;

                return GuidFormat;
            }
        }
    }

    public class ACL
    {
        [JsonIgnore]
        public int Id { get; set; }

        [NotMapped]
        public IList<string> ReadUsers { get; set; }

        [NotMapped]
        public IList<string> ReadGroups { get; set; }
    }

    public class User
    {
        public IList<string> Name { get; set; }
    }

    public class Group
    {
        public IList<string> Name { get; set; }
    }

    public class Attributes
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        //public class AttributesValidator: AbstractValidator<Attributes>
        //{
        //    public AttributesValidator()
        //    {
        //        RuleForEa
        //    }
        //}
    }
}
