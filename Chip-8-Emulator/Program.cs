using System;
using System.Buffers.Binary;
using System.IO;

namespace Chip_8_Emulator
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(@"games/GUESS", FileMode.Open)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var opcode = (ushort)(reader.ReadByte() << 8 | reader.ReadByte());
                    Console.WriteLine($"{opcode.ToString("X4")}");
                }

            }

            Console.ReadLine();
        }
    }
}
