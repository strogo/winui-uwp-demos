using System;
using System.ComponentModel.DataAnnotations;

namespace LayoutDemo {
    public class Data4 {
        const string NameGroup = "<Name>";
        const string TabbedGroup = "{Tabs}";
        const string JobGroup = "Job";
        const string JobGroupPath = TabbedGroup + "/" + JobGroup;
        const string ContactGroup = "Contact";
        const string ContactGroupPath = TabbedGroup + "/" + ContactGroup;
        const string AddressGroup = "Address";
        const string AddressGroupPath = ContactGroupPath + "/" + AddressGroup;
        const string PersonalGroup = "Personal-";

        [Display(GroupName = AddressGroupPath, ShortName = "")]
        public string AddressLine1 { get; set; }
        [Display(GroupName = AddressGroupPath, ShortName = "")]
        public string AddressLine2 { get; set; }
        [Display(GroupName = PersonalGroup, Name = "Birth date")]
        public DateTime BirthDate { get; set; }
        [Display(GroupName = ContactGroupPath, Order = 21)]
        public string Email { get; set; }
        [Display(GroupName = NameGroup, Name = "First name", Order = 0)]
        public string FirstName { get; set; }
        [Display(GroupName = PersonalGroup, Order = 3)]
        public Gender Gender { get; set; }
        [Display(GroupName = JobGroupPath, Order = 1)]
        public string Group { get; set; }
        [Display(GroupName = JobGroupPath, Name = "Hire date")]
        public DateTime HireDate { get; set; }
        [Display(GroupName = NameGroup, Name = "Last name")]
        public string LastName { get; set; }
        [Display(GroupName = ContactGroupPath, Order = 2), DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Display(GroupName = JobGroupPath), DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(GroupName = JobGroupPath, Order = 11)]
        public string Title { get; set; }

        public override string ToString() {
            return "Advanced grouped layout";
        }
    }
}
