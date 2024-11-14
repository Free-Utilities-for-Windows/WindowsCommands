using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using WindowsCommands.Logger;

namespace WindowsCommands;

public static class Sha256Hasher
{
    private static readonly uint[] Constants = new uint[64]
    {
        0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
        0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
        0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
        0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
        0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
        0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
        0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
        0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
    };

    private static uint[] _squareRoot = new uint[8]
    {
        0x6a09e667,
        0xbb67ae85,
        0x3c6ef372,
        0xa54ff53a,
        0x510e527f,
        0x9b05688c,
        0x1f83d9ab,
        0x5be0cd19
    };

    private static uint RotateRight(uint value, int bits) => (value >> bits) | (value << (32 - bits));

    private static uint ShiftRight(uint value, int bits) => value >> bits;

    private static uint Ch(uint x, uint y, uint z) => (x & y) ^ (~x & z);

    private static uint Maj(uint x, uint y, uint z) => (x & y) ^ (x & z) ^ (y & z);

    private static uint Sigma1(uint x) => RotateRight(x, 6) ^ RotateRight(x, 11) ^ RotateRight(x, 25);

    private static uint Sigma0(uint x) => RotateRight(x, 2) ^ RotateRight(x, 13) ^ RotateRight(x, 22);

    public static List<byte> Preprocessing(string input)
    {
        var data = new List<byte>(Encoding.UTF8.GetBytes(input));
        long len = data.Count * 8;

        data.Add(0x80);

        while ((data.Count + 8) % 64 != 0)
        {
            data.Add(0x00);
        }

        for (int i = 0; i < 8; i++)
        {
            data.Add((byte)((len >> (56 - 8 * i)) & 0xff));
        }

        return data;
    }

    public static void Hash(List<byte> data)
    {
        for (int j = 0; j < data.Count; j += 64)
        {
            var w = new uint[64];
            for (int i = 0; i < 16; i++)
            {
                w[i] = (uint)(data[j + 4 * i] << 24 | data[j + 4 * i + 1] << 16 | data[j + 4 * i + 2] << 8 |
                              data[j + 4 * i + 3]);
            }

            for (int i = 16; i < 64; i++)
            {
                var s0 = RotateRight(w[i - 15], 7) ^ RotateRight(w[i - 15], 18) ^ ShiftRight(w[i - 15], 3);
                var s1 = RotateRight(w[i - 2], 17) ^ RotateRight(w[i - 2], 19) ^ ShiftRight(w[i - 2], 10);
                w[i] = w[i - 16] + s0 + w[i - 7] + s1;
            }

            var a = _squareRoot[0];
            var b = _squareRoot[1];
            var c = _squareRoot[2];
            var d = _squareRoot[3];
            var e = _squareRoot[4];
            var f = _squareRoot[5];
            var g = _squareRoot[6];
            var h = _squareRoot[7];

            for (int i = 0; i < 64; i++)
            {
                var S1 = Sigma1(e);
                var S0 = Sigma0(a);
                var ch = Ch(e, f, g);
                var maj = Maj(a, b, c);
                var temp1 = h + S1 + ch + Constants[i] + w[i];
                var temp2 = S0 + maj;

                h = g;
                g = f;
                f = e;
                e = d + temp1;
                d = c;
                c = b;
                b = a;
                a = temp1 + temp2;
            }

            _squareRoot[0] += a;
            _squareRoot[1] += b;
            _squareRoot[2] += c;
            _squareRoot[3] += d;
            _squareRoot[4] += e;
            _squareRoot[5] += f;
            _squareRoot[6] += g;
            _squareRoot[7] += h;
        }
    }

    public static string ComputeHash(string input)
    {
        try
        {
            var data = Preprocessing(input);
            Hash(data);
            StringBuilder sb = new StringBuilder();
            foreach (var part in _squareRoot)
            {
                sb.AppendFormat("{0:x8}", part);
            }
            string hash = sb.ToString();
            StaticFileLogger.LogInformation($"Computed SHA-256 hash for input '{input}': {hash}");
            return hash;
        }
        catch (Exception ex)
        {
            string errorMessage = $"An error occurred while computing SHA-256 hash: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
            return null;
        }
    }

    public static void WriteHashToFile(string filePath, string hash)
    {
        try
        {
            File.WriteAllText(filePath, hash);
            StaticFileLogger.LogInformation($"Hash written to file: {filePath}");
        }
        catch (Exception ex)
        {
            string errorMessage = $"An error occurred while writing hash to file: {ex.Message}";
            Console.WriteLine(errorMessage);
            StaticFileLogger.LogError(errorMessage);
        }
    }
}