using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YenWeb.Models
{
    public class YenInputViewmodel : IValidatableObject
    {

        public YenInputViewmodel()
        {
            MirrorArcs = true;
        }

        [Required]
        [Display(Name ="CSV Input")]
        public string CsvString { get; set; }

        [Display(Name ="Minimum lenght")]
        public double MinimumLength { get; set; }

        [Display(Name ="Maximum length")]
        public double MaximumLength { get; set; }

        [Display(Name ="Auto-Insert Reverse Arcs")]
        public bool MirrorArcs { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MaximumLength <= 0)
            {
                yield return new ValidationResult("Maximum length must be positive", new string[] { "MaximumLength" });
            }
            if (MinimumLength < 0)
            {
                yield return new ValidationResult("Minimum length must be non-negative", new string[] { nameof(MinimumLength) });
            }
            if (MaximumLength < MinimumLength)
            {
                yield return new ValidationResult("Maximum length must be greater or equal to Minimum length", 
                    new string[] { nameof(MinimumLength), nameof(MaximumLength)});

                if(string.IsNullOrWhiteSpace(CsvString.Trim()))
                {
                    yield return new ValidationResult("Please provide CSV Input");
                }
        }
    }
}
}
