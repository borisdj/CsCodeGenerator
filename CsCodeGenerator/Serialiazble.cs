using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CsCodeGenerator
{
    public abstract class Serialiazble
    {
        private StringBuilder m_Builder = new StringBuilder();
        protected StringBuilder Builder => m_Builder;

        protected void AppendJoin<T>(string separator, IEnumerable<T> collection)
        {
            using (var en = collection.GetEnumerator())
            {
                if (!en.MoveNext())
                {
                    return;
                }

                var firstValue = en.Current;

                if (!en.MoveNext())
                {
                    m_Builder.Append(firstValue);
                    return;
                }

                m_Builder.Append(firstValue);

                do
                {
                    m_Builder.Append(separator);
                    m_Builder.Append(en.Current);
                }
                while (en.MoveNext());

            }
        }

    }
}
