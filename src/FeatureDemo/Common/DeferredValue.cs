using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Core;

namespace FeatureDemo.Common {
    public class DeferredValue<T> {
        object valueLock = new object();
        Func<T> value;
        bool valueCached;
        T cachedValue;

        public DeferredValue(Func<T> value) {
            this.value = value;
        }
        public T Value {
            get {
                lock(valueLock) {
                    if(!valueCached) {
                        valueCached = true;
                        cachedValue = value();
                    }
                    return cachedValue;
                }
            }
        }
        public Func<T> ValueAsFunc {
            get {
                return () => Value;
            }
        }
    }
}
