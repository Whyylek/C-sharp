using System;

class Matrix
{
    private int[,] data;
    public int Rows { get; }
    public int Columns { get; }

    public Matrix(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Rows and columns must be positive.");
        Rows = rows;
        Columns = columns;
        data = new int[rows, columns];
    }

    public int this[int i, int j]
    {
        get => data[i, j];
        set => data[i, j] = value;
    }

    public static Matrix operator +(Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
            throw new InvalidOperationException("Matrices dimensions must match for addition.");
        var result = new Matrix(a.Rows, a.Columns);
        for (int i = 0; i < a.Rows; i++)
            for (int j = 0; j < a.Columns; j++)
                result[i, j] = a[i, j] + b[i, j];
        return result;
    }

    public static Matrix operator -(Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
            throw new InvalidOperationException("Matrices dimensions must match for subtraction.");
        var result = new Matrix(a.Rows, a.Columns);
        for (int i = 0; i < a.Rows; i++)
            for (int j = 0; j < a.Columns; j++)
                result[i, j] = a[i, j] - b[i, j];
        return result;
    }

    public static Matrix operator *(Matrix a, Matrix b)
    {
        if (a.Columns != b.Rows)
            throw new InvalidOperationException("Number of columns in the first matrix must equal the number of rows in the second matrix.");
        var result = new Matrix(a.Rows, b.Columns);
        for (int i = 0; i < a.Rows; i++)
            for (int j = 0; j < b.Columns; j++)
                for (int k = 0; k < a.Columns; k++)
                    result[i, j] += a[i, k] * b[k, j];
        return result;
    }

    public static Matrix operator *(Matrix a, int scalar)
    {
        var result = new Matrix(a.Rows, a.Columns);
        for (int i = 0; i < a.Rows; i++)
            for (int j = 0; j < a.Columns; j++)
                result[i, j] = a[i, j] * scalar;
        return result;
    }

    public static Matrix operator *(int scalar, Matrix a) => a * scalar;

    public static Vector operator *(Matrix a, Vector v)
    {
        if (a.Columns != v.Size)
            throw new InvalidOperationException("Number of columns in the matrix must equal the size of the vector.");
        var result = new Vector(a.Rows);
        for (int i = 0; i < a.Rows; i++)
            for (int j = 0; j < a.Columns; j++)
                result[i] += a[i, j] * v[j];
        return result;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Matrix other || Rows != other.Rows || Columns != other.Columns)
            return false;
        for (int i = 0; i < Rows; i++)
            for (int j = 0; j < Columns; j++)
                if (data[i, j] != other[i, j])
                    return false;
        return true;
    }

    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }

    public static bool operator ==(Matrix a, Matrix b) => Equals(a, b);
    public static bool operator !=(Matrix a, Matrix b) => !Equals(a, b);

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
                sb.Append(data[i, j]).Append(" ");
            sb.AppendLine();
        }
        return sb.ToString();
    }
}

class Vector
{
    private int[] data;
    public int Size { get; }

    public Vector(int size)
    {
        if (size <= 0)
            throw new ArgumentException("Size must be positive.");
        Size = size;
        data = new int[size];
    }

    public int this[int i]
    {
        get => data[i];
        set => data[i] = value;
    }

    public static Vector operator +(Vector a, Vector b)
    {
        if (a.Size != b.Size)
            throw new InvalidOperationException("Vectors must have the same size for addition.");
        var result = new Vector(a.Size);
        for (int i = 0; i < a.Size; i++)
            result[i] = a[i] + b[i];
        return result;
    }

    public static Vector operator -(Vector a, Vector b)
    {
        if (a.Size != b.Size)
            throw new InvalidOperationException("Vectors must have the same size for subtraction.");
        var result = new Vector(a.Size);
        for (int i = 0; i < a.Size; i++)
            result[i] = a[i] - b[i];
        return result;
    }

    public static Vector operator *(Vector a, int scalar)
    {
        var result = new Vector(a.Size);
        for (int i = 0; i < a.Size; i++)
            result[i] = a[i] * scalar;
        return result;
    }

    public static Vector operator *(int scalar, Vector a) => a * scalar;

    public override bool Equals(object obj)
    {
        if (obj is not Vector other || Size != other.Size)
            return false;
        for (int i = 0; i < Size; i++)
            if (data[i] != other[i])
                return false;
        return true;
    }

    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }

    public static bool operator ==(Vector a, Vector b) => Equals(a, b);
    public static bool operator !=(Vector a, Vector b) => !Equals(a, b);

    public override string ToString()
    {
        return "[" + string.Join(", ", data) + "]";
    }
}

class Program
{
    static void Main()
    {
        
        Console.WriteLine("=== Matrix Examples ===");

        var matrix1 = new Matrix(2, 3);
        matrix1[0, 0] = 1; matrix1[0, 1] = 2; matrix1[0, 2] = 3;
        matrix1[1, 0] = 4; matrix1[1, 1] = 5; matrix1[1, 2] = 6;

        var matrix2 = new Matrix(2, 3);
        matrix2[0, 0] = 7; matrix2[0, 1] = 8; matrix2[0, 2] = 9;
        matrix2[1, 0] = 10; matrix2[1, 1] = 11; matrix2[1, 2] = 12;
      
        Console.WriteLine("Matrix1:");
        Console.WriteLine(matrix1);

        Console.WriteLine("Matrix2:");
        Console.WriteLine(matrix2);

        
        var matrixSum = matrix1 + matrix2;
        Console.WriteLine("Matrix1 + Matrix2:");
        Console.WriteLine(matrixSum);

        
        var matrixDiff = matrix1 - matrix2;
        Console.WriteLine("Matrix1 - Matrix2:");
        Console.WriteLine(matrixDiff);

        
        var matrix3 = new Matrix(3, 2);
        matrix3[0, 0] = 7; matrix3[0, 1] = 8;
        matrix3[1, 0] = 9; matrix3[1, 1] = 10;
        matrix3[2, 0] = 11; matrix3[2, 1] = 12;

        Console.WriteLine("Matrix3:");
        Console.WriteLine(matrix3);

        var matrixProduct = matrix1 * matrix3;
        Console.WriteLine("Matrix1 * Matrix3:");
        Console.WriteLine(matrixProduct);

        
        var matrixScalarProduct = matrix1 * 2;
        Console.WriteLine("Matrix1 * 2:");
        Console.WriteLine(matrixScalarProduct);

        
        Console.WriteLine("Matrix1 == Matrix2: " + (matrix1 == matrix2));
        Console.WriteLine("Matrix1 != Matrix2: " + (matrix1 != matrix2));

        
        Console.WriteLine("\n=== Vector Examples ===");

        var vector1 = new Vector(3);
        vector1[0] = 1; vector1[1] = 2; vector1[2] = 3;

        var vector2 = new Vector(3);
        vector2[0] = 4; vector2[1] = 5; vector2[2] = 6;

        Console.WriteLine("Vector1: " + vector1);
        Console.WriteLine("Vector2: " + vector2);

        
        var vectorSum = vector1 + vector2;
        Console.WriteLine("Vector1 + Vector2: " + vectorSum);

        
        var vectorDiff = vector1 - vector2;
        Console.WriteLine("Vector1 - Vector2: " + vectorDiff);

     
        var vectorScalarProduct = vector1 * 3;
        Console.WriteLine("Vector1 * 3: " + vectorScalarProduct);

       
        var matrixVectorProduct = matrix1 * vector1;
        Console.WriteLine("Matrix1 * Vector1: " + matrixVectorProduct);

        
        Console.WriteLine("Vector1 == Vector2: " + (vector1 == vector2));
        Console.WriteLine("Vector1 != Vector2: " + (vector1 != vector2));
    }
}