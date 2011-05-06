using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeatureExtraction.Extractors;
using FeatureExtraction.Faces;

namespace FeatureExtraction.FeaturePoints
{
    public class FeaturePointData
    {
        public FaceData Data { get; set; }
        public FeaturePoints FeaturePoints { get; set; }

        public FeaturePointData()
        {
            this.FeaturePoints = new FeaturePoints();
        }

        public FeaturePointData(FaceData data, IEnumerable<double> featurePoints)
        {
            this.Data = data;
            this.FeaturePoints = new FeaturePoints(featurePoints);
        }
    }

    public class FeaturePoints : List<double>
    {
        public FeaturePoints()
            : base()
        {
        }
        public FeaturePoints(int capacity)
            : base(capacity)
        {
            for (int i = 0; i < capacity; i++)
                this.Add(0);
        }

        public FeaturePoints(IEnumerable<double> collection)
            : base(collection)
        {
        }

        public static FeaturePoints operator+(FeaturePoints p1, FeaturePoints p2)
        {
            return p1.ElementwiseAdd(p2);
        }

        public static  FeaturePoints operator-(FeaturePoints p1, FeaturePoints p2)
        {
            return p1.ElementwiseAdd(-p2);
        }

        public static FeaturePoints operator*(FeaturePoints p1, double s)
        {
            return p1.ElementwiseMultiplyScalar(s);
        }

        public static FeaturePoints operator/(FeaturePoints p1, double s)
        {
            if (s == 0) {
                throw new DivideByZeroException();
            }
            return p1.ElementwiseMultiplyScalar(1 / s);
        }

        public static FeaturePoints operator-(FeaturePoints p)
        {
            return p.Negative();
        }

        public FeaturePoints ElementwiseAdd(FeaturePoints p)
        {
            if (this.Count() != p.Count())
            {
                throw new ArgumentException("inconsistent length");
            }
            FeaturePoints ret = new FeaturePoints();
            for (int i = 0; i < this.Count(); i++)
            {
                ret.Add(this[i] + p[i]);
            }
            return ret;
        }

        public FeaturePoints ElementwiseSubtract(FeaturePoints p)
        {
            if (this.Count() != p.Count())
            {
                throw new ArgumentException("inconsistent length");
            }
            return ElementwiseAdd(-p);
        }

        public FeaturePoints ElementwiseMultiplyScalar(double s)
        {
            FeaturePoints ret = new FeaturePoints();
            for (int i = 0; i < this.Count(); i++) {
                ret.Add(this[i] * s);
            }
            return ret;
        }

        public FeaturePoints ElementwiseProd(FeaturePoints p)
        {
            FeaturePoints ret = new FeaturePoints();
            for (int i = 0; i < this.Count(); i++)
            {
                ret.Add(this[i] * p[i]);
            }
            return ret;
        }

        public FeaturePoints Negative()
        {
            FeaturePoints ret = new FeaturePoints();
            for(int i = 0; i < this.Count(); i++) {
                ret.Add(-this[i]);
            }
            return ret;
        }
    }
}
