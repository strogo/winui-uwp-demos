using System;
using System.ComponentModel.DataAnnotations;

namespace LayoutDemo {
    public class Data1 {
        public int IntProperty { get; set; }
        public int? NullableIntProperty { get; set; }
        public double DoubleProperty { get; set; }
        public double? NullableDoubleProperty { get; set; }
        public bool BoolProperty { get; set; }
        public bool? NullableBoolProperty { get; set; }
        public char CharProperty { get; set; }
        public char? NullableCharProperty { get; set; }
        public Gender EnumProperty { get; set; }
        [DisplayFormat(NullDisplayText = "Undefined")]
        public Gender? NullableEnumProperty { get; set; }
        public string StringProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
        public DateTime? NullableDateTimeProperty { get; set; }
        public decimal DecimalProperty { get; set; }
        public decimal? NullableDecimalProperty { get; set; }

        [DataType(DataType.Currency)]
        public decimal CurrencyProperty { get; set; }
        [DataType(DataType.MultilineText)]
        public string MultilineTextProperty { get; set; }
        [DataType(DataType.Password)]
        public string PasswordProperty { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumberProperty { get; set; }

        public override string ToString() {
            return "Supported data types";
        }
    }
}
