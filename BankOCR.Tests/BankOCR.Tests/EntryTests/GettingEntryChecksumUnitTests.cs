using System;
using BankOCR.ConsoleApp;
using FluentAssertions;
using Xunit;
using static LanguageExt.Prelude;

namespace BankOCR.Tests.FileLocationTests
{
    public class GettingEntryChecksumUnitTests
    {
        [Fact]
        public void ChecksumIsCorrectlyCounted()
        {
            //a
            var machineoutput = (FakeMachineOutput)FakeMachineOutput.FromActualValueOf(
                Array('6','6','4','3','7','1','4','9','5'));
            var lines = map(machineoutput.MachineOutput, (first,second,third) =>
            (
                (InputLine)InputLine.New(first),
                (InputLine)InputLine.New(second),
                (InputLine)InputLine.New(third)
            ));
            
            //aa
            var result = Entry.ParseFromLines(lines)
                .Bind(entry => Checksum.ForEntry(entry))
                .Map(checksum =>checksum.CheckValidity());
            
            //aaa
            result.Should().BeSuccess();
            result.Do(validity => validity.Should().Be(Checksum.Validity.Invalid)); 
        }
        
        [Fact]
        public void ChecksumIsCorrectlyCounted2()
        {
            //a
            var machineoutput = (FakeMachineOutput)FakeMachineOutput.FromActualValueOf(
                Array('4','5','7','5','0','8','0','0','0'));
            var lines = map(machineoutput.MachineOutput, (first,second,third) =>
            (
                (InputLine)InputLine.New(first),
                (InputLine)InputLine.New(second),
                (InputLine)InputLine.New(third)
            ));
            
            //aa
            var result = Entry.ParseFromLines(lines)
                .Bind(entry => Checksum.ForEntry(entry))
                .Map(checksum =>checksum.CheckValidity());
            
            //aaa
            result.Should().BeSuccess();
            result.Do(validity => validity.Should().Be(Checksum.Validity.Valid)); 
        }
    }
}