using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC_Tema1
{
    
    internal class Node(int id)
    {
        private static readonly Random _random = new Random();
        public int Id { get; } = id;
        public string ?message { get;  set; }
        public int ?destination {  get; set; }
        
        public Node nextNode {  get; set; }

        ConcurrentQueue<TokenRingFrame> buffer = new ConcurrentQueue<TokenRingFrame>();
        public void Add(TokenRingFrame token)
        {
            buffer.Enqueue(token);
        }

        public async void RunExecution()
        {
            while (true)
            {

                bool hasRecived = buffer.TryDequeue(out TokenRingFrame recivedToken);
                if (hasRecived)
                {
                    Process(recivedToken);
                }


            }

        }

        public void Process(TokenRingFrame token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            if (token.IsTokenFree)
            {
                if (message == null)
                {
                    nextNode.Add(token);
                    return;
                }

                token.IsTokenFree = false;
                token.SourceId = this.Id;
                token.DestinationId = Convert.ToInt16(destination);
                token.Data = message;

                token.Crc = CalculateCrc(token.Data);

                nextNode.Add(token);
                return;
            }

            // Token is carrying data: forward it, but destination should validate & "consume"
            if (token.DestinationId == this.Id && !token.DataCopied)
            {
                bool ok = ValidateCrc(token.Data, token.Crc);

                // In a real token ring you'd copy data, set flags, etc.
                token.DestinationFound = true;
                token.DataCopied = true;

                Debug.WriteLine(
                    ok
                        ? $"Node {Id}: CRC OK from {token.SourceId}, data='{token.Data}'"
                        : $"Node {Id}: CRC FAIL from {token.SourceId}, data considered CORRUPT");

                // Free the token after destination processed it (simplified behavior)
                token.IsTokenFree = true;
                token.Data = null;
                token.Crc = null;
            }

            nextNode.Add(token);
        }

        private string CalculateCrc(string data)
        {
            // Generator polynomial: 1011 (degree = 3 => remainder/FCS has 3 bits)
            int[] generator = { 1, 0, 1, 1 };
            int degree = generator.Length - 1;

            // Data as bits (NO zeros appended here)
            string dataBits = StringToBitsRaw(data);

            // Working buffer = dataBits + degree zeros
            int[] dividend = new int[dataBits.Length + degree];
            for (int i = 0; i < dataBits.Length; i++)
                dividend[i] = dataBits[i] - '0';

            // Polynomial long division (XOR)
            // Only need to process positions where the generator still "fits"
            for (int i = 0; i <= dividend.Length - generator.Length; i++)
            {
                if (dividend[i] == 0)
                    continue;

                for (int j = 0; j < generator.Length; j++)
                    dividend[i + j] ^= generator[j];
            }

            // Remainder = last 'degree' bits
            StringBuilder fcs = new();
            for (int i = dividend.Length - degree; i < dividend.Length; i++)
                fcs.Append(dividend[i]);

            return fcs.ToString(); // only the CRC/FCS
        }

        private static string StringToBitsRaw(string message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            StringBuilder bits = new();

            foreach (byte b in bytes)
                bits.Append(Convert.ToString(b, 2).PadLeft(8, '0'));

            return bits.ToString();
        }

        private static bool ValidateCrc(string data, string receivedFcs)
        {
            int[] generator = { 1, 0, 1, 1 };
            int degree = generator.Length - 1;

            if (receivedFcs == null || receivedFcs.Length != degree)
                return false;

            string dataBits = StringToBitsRaw(data);
            string receivedBits = dataBits + receivedFcs; 

            int[] dividend = new int[receivedBits.Length];
            for (int i = 0; i < receivedBits.Length; i++)
                dividend[i] = receivedBits[i] - '0';

            for (int i = 0; i <= dividend.Length - generator.Length; i++)
            {
                if (dividend[i] == 0)
                    continue;

                for (int j = 0; j < generator.Length; j++)
                    dividend[i + j] ^= generator[j];
            }

            for (int i = dividend.Length - degree; i < dividend.Length; i++)
            {
                if (dividend[i] != 0)
                    return false;
            }

            return true;
        }

        //internal void Message(string v)
        //{
        //    throw new NotImplementedException();
        //}
    }


    }







