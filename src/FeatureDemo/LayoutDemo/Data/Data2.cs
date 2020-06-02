using System.ComponentModel.DataAnnotations;

namespace LayoutDemo {
    public class Data2 {
        [Display(AutoGenerateField = false)]
        public int ID { get; set; }

        public int Age { get; set; }
        [Editable(false)]
        public string Employer { get; set; }
        [Display(Name = "First name", Order = 0), Required]
        public string FirstName { get; set; }
        [Display(Name = "Full name", Order = 2)]
        public string FullName { get { return FirstName + " " + LastName; } }
        [Display(ShortName = "Sex", Order = 3)]
        public Gender Gender { get; set; }
        [Display(Name = "Last name", Order = 1), Required]
        public string LastName { get; set; }
        [Editable(false)]
        public string SSN { get; set; }

        public override string ToString() {
            return "Attribute support";
        }
    }
}
