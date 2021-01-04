using System;
using System.Collections.Generic;
using System.Text;

class PacketBuffer : IDisposable
{
    List<byte> _bufferList;
    byte[] _readbuffer;
    int _readpos;
    bool _buffupdate = false;

    public PacketBuffer()
    {
        _bufferList = new List<byte>();
        _readpos = 0;
    }
    public int GetReadPost()
    {
        return _readpos;
    }
    public byte[] ToArray()
    {
        return _bufferList.ToArray();
    }
    public int Count()
    {
        return _bufferList.Count;
    }
    public int Length()
    {
        return Count() - _readpos;
    }
    public void Clear()
    {
        _bufferList.Clear();
        _readpos = 0;
    }

    //Write Data
    public void WriteBytes(byte[] input)
    {
        _bufferList.AddRange(input);
        _buffupdate = true;
    }
    public void WriteByte(byte input)
    {
        _bufferList.Add(input);
        _buffupdate = true;
    }
    public void WriteInteger(int input)
    {
        _bufferList.AddRange(BitConverter.GetBytes(input));
        _buffupdate = true;
    }
    public void WriteFloat(float input)
    {
        _bufferList.AddRange(BitConverter.GetBytes(input));
        _buffupdate = true;
    }
    public void WriteString(string input)
    {
        _bufferList.AddRange(BitConverter.GetBytes(input.Length));
        _bufferList.AddRange(Encoding.ASCII.GetBytes(input));
        _buffupdate = true;
    }

    // Read Data
    public int ReadInteger(bool peek = true)
    {
        if (_bufferList.Count > _readpos)
        {
            if (_buffupdate)
            {
                _readbuffer = _bufferList.ToArray();
                _buffupdate = false;
            }

            int value = BitConverter.ToInt32(_readbuffer, _readpos);
            if (peek & _bufferList.Count > _readpos)
            {
                _readpos += 4;
            }
            return value;
        }
        else
        {
            throw new Exception("Buffer is pas its limit!");
        }
    }
    public float ReadFloat(bool peek = true)
    {
        if (_bufferList.Count > _readpos)
        {
            if (_buffupdate)
            {
                _readbuffer = _bufferList.ToArray();
                _buffupdate = false;
            }

            float value = BitConverter.ToSingle(_readbuffer, _readpos);
            if (peek & _bufferList.Count > _readpos)
            {
                _readpos += 4;
            }
            return value;
        }
        else
        {
            throw new Exception("Buffer is pas its limit!");
        }
    }
    public byte ReadByte(bool peek = true)
    {
        if (_bufferList.Count > _readpos)
        {
            if (_buffupdate)
            {
                _readbuffer = _bufferList.ToArray();
                _buffupdate = false;
            }

            byte value = _readbuffer[_readpos];
            if (peek & _bufferList.Count > _readpos)
            {
                _readpos += 1;
            }
            return value;
        }
        else
        {
            throw new Exception("Buffer is pas its limit!");
        }
    }
    public byte[] ReadBytes(int Length, bool peek = true)
    {

        if (_buffupdate)
        {
            _readbuffer = _bufferList.ToArray();
            _buffupdate = false;
        }

        byte[] value = _bufferList.GetRange(_readpos, Length).ToArray();
        if (peek & _bufferList.Count > _readpos)
        {
            _readpos += Length;
        }
        return value;
    }
    public string ReadString(bool peek = true)
    {
        int length = ReadInteger(true);
        if (_buffupdate)
        {
            _readbuffer = _bufferList.ToArray();
            _buffupdate = false;
        }

        string value = Encoding.ASCII.GetString(_readbuffer, _readpos, length);
        if (peek & _bufferList.Count > _readpos)
        {
            _readpos += length;
        }
        return value;
    }
    private bool disposedValue = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _bufferList.Clear();
            }
            _readpos = 0;
        }
        disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}