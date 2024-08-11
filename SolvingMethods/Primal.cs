using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Project.SolvingMethods
{
    internal class Primal
    {
        private string problemType;
        private int[] objFunction;
        private int[,] constraintsCoefficients;
        private int[] rhsConstraints;
        private string[] signRestrictions;
        private float[,] tablua;

        public string ProblemType { get => problemType; set => problemType = value; }
        public int[] ObjFunction { get => objFunction; set => objFunction = value; }
        public int[,] ConstraintsCoefficients { get => constraintsCoefficients; set => constraintsCoefficients = value; }
        public int[] RhsConstraints { get => rhsConstraints; set => rhsConstraints = value; }
        public string[] SignRestrictions { get => signRestrictions; set => signRestrictions = value; }

        public Primal(string problemType, int[] objFunction, int[,] constraintsCoefficients, int[] rhsConstraints, string[] signRestrictions)
        {
            this.problemType = problemType;
            this.objFunction = objFunction;
            this.constraintsCoefficients = constraintsCoefficients;
            this.rhsConstraints = rhsConstraints;
            this.signRestrictions = signRestrictions;
        }

        public void Solve()
        {
            //1. Shape tablua
            //1.1 Put constraints into optimal shape
            //1.2 Add helper variables
            /*
             * obj obj obj rhs
             * con con con rhs
             * con con con rhs
             * con con con rhs
             */
            //2. Test on descision variables
            //3. Ratio test
            //4. Math on tablua
            //5. Repaet 2-4 untill optimal
            //Tabluafy();
            //Console.WriteLine(ToString());

            Prepare();
            bool optimal = false;

            while (!optimal)
            {
                int chosen = 0;
                //performs checks on the obj values to select the largest negative value.

                for (int i = 0; i < tablua.GetLength(1); i++)
                {
                    if (tablua[0, i] < tablua[0, chosen])
                    {
                        chosen = i;
                    }
                }

                //if the value at position 0, i in the tablue is smaler than zero, then calculations wil be performed on it
                if (tablua[0, chosen] < 0)
                {
                    //performs calculations on the tableua
                    int pos = 1;

                    for (int i = 1; i < tablua.GetLength(0); i++)
                    {
                        //performs the ratio test on rhs values and selects the row with the lowest non-negative ratio
                        if (((tablua[chosen, i] < tablua[chosen, pos]) && (tablua[chosen, i] >= 0)))
                        {
                            pos = i;
                        }
                    }
                }
            }
        }

        //Creates a NxM matrix that contains the lpr problem
        private Prepare()
        {
            float[,] constraints = new float[constraintsCoefficients.GetLength(0), constraintsCoefficients.GetLength(1) + 2];

            for (int i = 0; i < constraintsCoefficients.GetLength(0); i++)
            {
                for (int j = 0; j < constraintsCoefficients.GetLength(1); j++)
                {
                    constraints[i, j] = constraintsCoefficients[i, j];
                }
                constraints[i, constraintsCoefficients.GetLength(1)] = 0;
                constraints[i, constraintsCoefficients.GetLength(1) + 1] = rhsConstraints[i];
            }

            for (int i = 0; i < constraints.GetLength(0); i++)
            {
                for (int j = 0; j < constraints.GetLength(1); j++)
                {
                    //Console.Write(constraints[i, j]+" ");
                }
                // Console.WriteLine();
            }

            float[] obj = new float[constraints.GetLength(1)];

            for (int i = 0; i < objFunction.GetLength(0); i++)
            {
                obj[i] = objFunction[i] * (-1);
                Console.Write(obj[i] + " ");

            }

            for (int i = objFunction.GetLength(0); i < constraints.GetLength(1) - 1; i++)
            {
                obj[i] = 0;
                //Console.Write(obj[i] + " ");
            }

            //Console.WriteLine();
            //Console.WriteLine("-----------");

            tablua = new float[constraints.GetLength(0) + 1, constraints.GetLength(1)];

            for (int i = 0; i < obj.GetLength(0) - 1; i++)
            {
                tablua[0, i] = obj[i];
                Console.Write(tablua[0, i] + " ");
            }

            for (int i = 1; i < constraints.GetLength(0); i++)
            {
                Console.WriteLine();

                for (int j = 1; j < constraints.GetLength(1); j++)
                {
                    tablua[i, j - 1] = constraints[i - 1, j - 1];
                    Console.Write(tablua[i, j - 1] + " ");
                }
            }
        }

        //Transforms the tableua into a string that can then be written to console
        public override string ToString()
        {
            string myString = string.Empty;

            for (int i = 0; i < tablua.GetLength(0); i++)
            {
                for (int j = 0; j < tablua.GetLength(1); j++)
                {
                    myString += tablua[i, j] + " ";
                }
                myString += "\n";
            }
            return myString;
        }
    }
}