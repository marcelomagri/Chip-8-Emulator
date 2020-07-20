using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;

namespace Chip_8_Emulator
{
    class Program
    {

        const ushort chip8StartAdress = 0x200;

        static void Main(string[] args)
        {
            CPU cpu = new CPU();

            using (BinaryReader reader = new BinaryReader(new FileStream(@"games/GUESS", FileMode.Open)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    // for some reason, it is reading low-endian
                    // being Chip-8 big-endian, we shift the bytes to make the order right
                    var opcode = (ushort)(reader.ReadByte() << 8 | reader.ReadByte());
                    //Console.WriteLine($"{opcode.ToString("X4")}");
                    cpu.ExecuteOpcode(opcode);
                }

            }

            Console.ReadLine();
        }
    }

    class CPU
    {
        byte[] memory = new byte[4096];
        byte[] registers = new byte[16];
        ushort I = 0;
        Stack<ushort> Stack = new Stack<ushort>();

        public CPU()
        {

        }

        public void ExecuteOpcode(ushort opcode)
        {
            var instruction = (ushort)(opcode & 0xF000);

            switch (instruction)
            {
                case 0x0000:
                    if (opcode == 0x00e0)
                    {
                        // clears the screen
                        Console.WriteLine($"{opcode.ToString("X4")}\tCLS");
                    }
                    else
                    {
                        Console.WriteLine($"{opcode.ToString("X4")}\tUnknown Opcode");
                    }
                    break;
                case 0x1000:
                    I = (ushort)(opcode & 0x0FFF);
                    Console.WriteLine($"{opcode.ToString("X4")}\tJP\t{(opcode & 0x0FFF).ToString("X4")}");
                    break;
                case 0x2000:
                    Stack.Push(I);
                    I = (ushort)(opcode & 0x0FFF);
                    Console.WriteLine($"{opcode.ToString("X4")}\tCALL\t{(opcode & 0x0FFF).ToString("X4")}");
                    break;
                case 0x3000:
                    if (registers[(opcode & 0x0F00) >> 8] == (opcode & 0x00FF)) I += 2;
                    Console.WriteLine($"{opcode.ToString("X4")}\tSE\t{(opcode & 0x0F00).ToString("X4")},{(opcode & 0x00FF).ToString("X4")}");
                    break;
                case 0x4000:
                    if (registers[(opcode & 0x0F00) >> 8] != (opcode & 0x00FF)) I += 2;
                    Console.WriteLine($"{opcode.ToString("X4")}\tSNE\t{((opcode & 0x0F00) >> 8).ToString("X4")},{(opcode & 0x00FF).ToString("X4")}");
                    break;
                case 0x5000:
                    if (registers[(opcode & 0x0F00) >> 8] == (registers[opcode & 0x00F0])) I += 2;
                    Console.WriteLine($"{opcode.ToString("X4")}\tSE\t{((opcode & 0x0F00) >> 8).ToString("X4")},{(opcode & 0x00F0).ToString("X4")}");
                    break;
                case 0x6000:
                    registers[(opcode & 0x0F00) >> 8] = (byte)(opcode & 0x00FF);
                    Console.WriteLine($"{opcode.ToString("X4")}\tLD\t{((opcode & 0x0F00) >> 8).ToString("X4")},{(opcode & 0x00FF).ToString("X4")}");
                    break;
                default:
                    Console.WriteLine($"{opcode.ToString("X4")}\tUnknown Opcode");
                    break;
            }
        }
    }
}
