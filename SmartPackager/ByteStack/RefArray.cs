using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPackager.ByteStack
{

    /// <summary>
    /// Хранит в себе массив ссылок на объекты
    /// </summary>
    internal struct RefArray
    {
        private List<RefPoint> List;

        public void AddRef(RefPoint point)
        {
            if (List == null)
                List = new List<RefPoint>(25);

            List.Add(point);
        }

        public bool Exists(object val,out RefPoint point)
        {
            if (List == null)
            {
                point = default;
                return false;
            }

            for(int i = 0; i < List.Count; i++)
            {
                if (List[i].Data == val)
                {
                    point = List[i];
                    return true;
                }
            }

            point = default;
            return false;
        }

        public object GetObject(int pos)
        {
            for(int i = 0; i < List.Count; i++)
            {
                if (List[i].Point == pos)
                {
                    return List[i].Data;
                }
            }

            throw new Exception("Ошибка поиска ссылки, возможно данные повреждены!");
        }
    }
}
