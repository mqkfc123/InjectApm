﻿using SkyApm.Abstractions.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Ids
{

    public class ID : IEquatable<ID>
    {
        private readonly long _part1;
        private readonly long _part2;
        private readonly long _part3;
        private string _encoding;

        public bool IsValid { get; }

        public string Encode => _encoding ?? (_encoding = ToString());

        public ID(long part1, long part2, long part3)
        {
            _part1 = part1;
            _part2 = part2;
            _part3 = part3;
            IsValid = true;
        }

        public ID(string encodingString)
        {
            if (encodingString == null)
            {
                throw new ArgumentNullException(nameof(encodingString));
            }
            string[] idParts = encodingString.Split("\\.".ToCharArray(), 3);
            for (int part = 0; part < 3; part++)
            {
                if (part == 0)
                {
                    IsValid = long.TryParse(idParts[part], out _part1);
                }
                else if (part == 1)
                {
                    IsValid = long.TryParse(idParts[part], out _part2);
                }
                else
                {
                    IsValid = long.TryParse(idParts[part], out _part3);
                }
                if (!IsValid)
                {
                    break;
                }
            }
        }

        public override string ToString()
        {
            return $"{_part1}.{_part2}.{_part3}";
        }

        public override int GetHashCode()
        {
            var result = (int)(_part1 ^ (_part1 >> 32));
            result = 31 * result + (int)(_part2 ^ (_part2 >> 32));
            result = 31 * result + (int)(_part3 ^ (_part3 >> 32));
            return result;
        }

        public override bool Equals(object obj)
        {
            var id = obj as ID;
            return Equals(id);
        }

        public bool Equals(ID other)
        {
            if (other == null)
                return false;
            if (this == other)
                return true;
            if (_part1 != other._part1)
                return false;
            if (_part2 != other._part2)
                return false;
            return _part3 == other._part3;
        }

        public UniqueIdRequest Transform()
        {
            var uniqueId = new UniqueIdRequest { Part1 = _part1, Part2 = _part2, Part3 = _part3 };
            return uniqueId;
        }
    }
}
