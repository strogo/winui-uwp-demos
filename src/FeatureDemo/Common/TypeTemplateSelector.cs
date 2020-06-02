using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using System.Reflection;

namespace FeatureDemo.Common {
    public static class RunTimePropertyHelper {
        public static object GetRunTimePropertyValue(this object o, string path) {
            string[] properties = path.Split('.');
            foreach(string property in properties) {
                o = o.GetType().GetRuntimeProperty(property).GetValue(o);
            }
            return o;
        }
    }
    public class DataTemplateDictionary : Dictionary<string, DataTemplate> {}
    [ContentProperty(Name = "Templates")]
    public class TypeTemplateSelector : DataTemplateSelector {
        public TypeTemplateSelector() {
            Templates = new DataTemplateDictionary();
        }
        public DataTemplateDictionary Templates { get; set; }
        public string ConditionMember { get; set; }
        
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) {
            if(item == null) return null;
            string type = item.GetType().Name;
            string condition;
            if(ConditionMember == null) {
                condition = string.Empty;
            } else {
                object v = item.GetRunTimePropertyValue(ConditionMember);
                condition = ":" + (v == null ? "NULL" : v.ToString());
            }
            IEnumerator<DataTemplate> t = (from p in Templates where p.Key == type || p.Key == condition || p.Key == type + condition select p.Value).GetEnumerator();
            return t.MoveNext() ? t.Current : null;
        }
    }
    public class StyleDictionary : Dictionary<string, Style> { }
    [ContentProperty(Name = "Styles")]
    public class TypeStyleSelector : StyleSelector {
        public TypeStyleSelector() {
            Styles = new StyleDictionary();
        }
        public StyleDictionary Styles { get; set; }
        public string ConditionMember { get; set; }
        protected override Style SelectStyleCore(object item, DependencyObject container) {
            if(item == null) return null;
            string type = item.GetType().Name;
            string condition;
            if(ConditionMember == null) {
                condition = string.Empty;
            } else {
                object v = item.GetRunTimePropertyValue(ConditionMember);
                condition = ":" + (v == null ? "NULL" : v.ToString());
            }
            IEnumerator<Style> t = (from p in Styles where p.Key == type || p.Key == condition || p.Key == type + condition select p.Value).GetEnumerator();
            return t.MoveNext() ? t.Current : null;
        }
    }
}
