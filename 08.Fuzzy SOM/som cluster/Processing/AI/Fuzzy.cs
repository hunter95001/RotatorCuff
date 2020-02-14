using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace som_cluster.Processing
{
    class Fuzzy
    {
        public Fuzzy()
        {
        }

        public double triangleMemberShip(double HIGH, double LOW, double INPUT)
        {
            double value = 0;
            double MID = Math.Round((HIGH + LOW) / 2);

            if (INPUT <= LOW && INPUT >= HIGH)
            {
                value = 0;
            }
            else if (INPUT > MID)
            {
                value = (HIGH - INPUT) / (HIGH - MID);
            }
            else if (INPUT < MID)
            {
                value = (INPUT - LOW) / (MID - LOW);
            }
            else if (INPUT == MID)
            {
                value = 1;
            }
            return Math.Round(value, 1);
        }

        public double MemberShip(double HIGH, double LOW, double INPUT)
        {

            double value = 0;
            double MID = HIGH;

            if (INPUT <= LOW && INPUT >= HIGH)
            {
                value = 0;
            }
            else if (INPUT > MID)
            {
                value = (HIGH - INPUT) / (HIGH - MID);
            }
            else if (INPUT < MID)
            {
                value = (INPUT - LOW) / (MID - LOW);
            }
            else if (INPUT == MID)
            {
                value = 1;
            }
            return Math.Round(value, 4);
        }


    }
}
