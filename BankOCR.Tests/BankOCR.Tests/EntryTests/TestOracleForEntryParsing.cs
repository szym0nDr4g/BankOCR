using System;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;

namespace BankOCR.Tests.FileLocationTests
{
    public struct FakeMachineOutput
    {
        public Arr<char> ActualOutputValue { get; private set; }
        public (Arr<char> FirstLine, Arr<char> SecondLine, Arr<char> ThirdLine) MachineOutput { get; private set; }
        
        public static Either<Error, FakeMachineOutput> FromActualValueOf(Arr<char> actualOutputValue)
        {
            if (actualOutputValue.Any(c => !char.IsDigit(c)))
                return Error.New("only digits allowed");

            if (actualOutputValue.Count != 9)
                return Error.New("number needs to have length of 9");

            var machineOutput = GetMachineOutput(actualOutputValue);

            return new FakeMachineOutput()
            {
                ActualOutputValue = actualOutputValue,
                MachineOutput = machineOutput
            };
        }

        public override string ToString()
        {
            return
                $"{ActualOutputValue}{System.Environment.NewLine}{String.Join(System.Environment.NewLine, String.Join("", MachineOutput.Item1), String.Join("", MachineOutput.Item2), String.Join("", MachineOutput.Item3))}{System.Environment.NewLine}";
        }

        private FakeMachineOutput(Arr<char> actualOutputValue,
            (Arr<char> FirstLine, Arr<char> SecondLine, Arr<char> ThirdLine) machineOutput)
        {
            ActualOutputValue = actualOutputValue;
            MachineOutput = machineOutput;
        }

        private static (Arr<char> FirstLine, Arr<char> SecondLine, Arr<char> ThirdLine) GetMachineOutput(Arr<char> actualOutputValue)
        {
            var firstLine = new Arr<char>();
            var secondLine = new Arr<char>();
            var thirdLine = new Arr<char>();
            
            foreach (var character in actualOutputValue)
            {
                if (character == '0')
                {
                    firstLine = firstLine.Add(' ').Add('_').Add(' ');
                    secondLine = secondLine.Add('|').Add(' ').Add('|');
                    thirdLine = thirdLine.Add('|').Add('_').Add('|');
                }
                else if (character == '1')
                {
                    firstLine = firstLine.Add(' ').Add(' ').Add(' ');
                    secondLine = secondLine.Add(' ').Add('|').Add(' ');
                    thirdLine = thirdLine.Add(' ').Add('|').Add(' ');
                }
                else if (character == '2')
                {
                    firstLine = firstLine.Add(' ').Add('_').Add(' ');
                    secondLine = secondLine.Add(' ').Add('_').Add('|');
                    thirdLine = thirdLine.Add('|').Add('_').Add(' ');
                }
                else if (character == '3')
                {
                    firstLine = firstLine.Add(' ').Add('_').Add(' ');
                    secondLine = secondLine.Add(' ').Add('_').Add('|');
                    thirdLine = thirdLine.Add(' ').Add('_').Add('|');
                }
                else if (character == '4')
                {
                    firstLine = firstLine.Add(' ').Add(' ').Add(' ');
                    secondLine = secondLine.Add('|').Add('_').Add('|');
                    thirdLine = thirdLine.Add(' ').Add(' ').Add('|');
                }
                else if (character == '5')
                {
                    firstLine = firstLine.Add(' ').Add('_').Add(' ');
                    secondLine = secondLine.Add('|').Add('_').Add(' ');
                    thirdLine = thirdLine.Add(' ').Add('_').Add('|');
                }
                else if (character == '6')
                {
                    firstLine = firstLine.Add(' ').Add('_').Add(' ');
                    secondLine = secondLine.Add('|').Add('_').Add(' ');
                    thirdLine = thirdLine.Add('|').Add('_').Add('|');
                }
                else if (character == '7')
                {
                    firstLine = firstLine.Add(' ').Add('_').Add(' ');
                    secondLine = secondLine.Add(' ').Add(' ').Add('|');
                    thirdLine = thirdLine.Add(' ').Add(' ').Add('|');
                }
                else if (character == '8')
                {
                    firstLine = firstLine.Add(' ').Add('_').Add(' ');
                    secondLine = secondLine.Add('|').Add('_').Add('|');
                    thirdLine = thirdLine.Add('|').Add('_').Add('|');
                }
                else if (character == '9')
                {
                    firstLine = firstLine.Add(' ').Add('_').Add(' ');
                    secondLine = secondLine.Add('|').Add('_').Add('|');
                    thirdLine = thirdLine.Add(' ').Add('_').Add('|');
                }
            }
            
            return (firstLine, secondLine, thirdLine);
        }
    }
}