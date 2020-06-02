using System;
using System.ComponentModel.DataAnnotations;

namespace LayoutDemo {
    public class Data3 {
        const string JobGroup = "Job";
        const string ContactGroup = "Contact";
        const string AddressGroup = "Address";
        const string PersonalGroup = "Personal";

        [Display(GroupName = AddressGroup, ShortName = "", Order = 4)]
        public string AddressLine1 { get; set; }
        [Display(GroupName = AddressGroup, ShortName = "")]
        public string AddressLine2 { get; set; }
        [Display(GroupName = PersonalGroup, Name = "Birth date")]
        public DateTime BirthDate { get; set; }
        [Display(GroupName = ContactGroup)]
        public string Email { get; set; }
        [Display(Name = "First name", Order = 0)]
        public string FirstName { get; set; }
        [Display(GroupName = PersonalGroup, Order = 5)]
        public Gender Gender { get; set; }
        [Display(GroupName = JobGroup, Order = 2)]
        public string Group { get; set; }
        [Display(GroupName = JobGroup, Name = "Hire date")]
        public DateTime HireDate { get; set; }
        [Display(Name = "Last name", Order = 1)]
        public string LastName { get; set; }
        [Display(GroupName = ContactGroup, Order = 3), DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Display(GroupName = JobGroup), DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(GroupName = JobGroup, Order = 21)]
        public string Title { get; set; }

        public override string ToString() {
            return "Grouped list";
        }
    }
}
