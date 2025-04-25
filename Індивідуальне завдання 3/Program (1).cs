using System;



public class Point : ICloneable
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public object Clone()
    {
        return new Point(X, Y);
    }

    public override string ToString()
    {
        return $"Point(X={X}, Y={Y})";
    }
}


public abstract class Shape
{
    public abstract double Area { get; }
}


public class Square : Shape
{
    public double Side { get; }

    public Square(double side)
    {
        Side = side;
    }

    public override double Area => Side * Side;

    public override string ToString()
    {
        return $"Square(Side={Side}, Area={Area})";
    }
}


public class Triangle : Shape
{
    public double Side1 { get; }
    public double Side2 { get; }
    public double Side3 { get; }

    public Triangle(double side1, double side2, double side3)
    {
        Side1 = side1;
        Side2 = side2;
        Side3 = side3;

        
        if (!IsValidTriangle())
            throw new ArgumentException("Invalid triangle sides");
    }

    private bool IsValidTriangle()
    {
        return Side1 + Side2 > Side3 && Side1 + Side3 > Side2 && Side2 + Side3 > Side1;
    }

    public override double Area
    {
        get
        {
            
            double s = (Side1 + Side2 + Side3) / 2;
            return Math.Sqrt(s * (s - Side1) * (s - Side2) * (s - Side3));
        }
    }

    public override string ToString()
    {
        return $"Triangle(Sides={Side1},{Side2},{Side3}, Area={Area})";
    }
}


public class Circle : Shape, ICloneable
{
    public Point Center { get; }
    public double Radius { get; }

    public Circle(Point center, double radius)
    {
        Center = center;
        Radius = radius;

        if (Radius <= 0)
            throw new ArgumentException("Radius must be positive");
    }

    public override double Area => Math.PI * Radius * Radius;

    public object Clone()
    {
        return new Circle((Point)Center.Clone(), Radius);
    }

    public override string ToString()
    {
        return $"Circle(Center={Center}, Radius={Radius}, Area={Area})";
    }
}

class Program
{
    static void Main()
    {

        Shape[] shapes = new Shape[]
         {
            new Square(5),
            new Triangle(3, 4, 5),
            new Circle(new Point(0, 0), 3),
            new Square(4),
            new Circle(new Point(1, 1), 2)
         };

       
        double totalArea = 0;
        foreach (var shape in shapes)
        {
            totalArea += shape.Area;
        }
        Console.WriteLine($"Total area of all shapes: {totalArea:F2}");

     
        Circle[] circles = new Circle[]
        {
            new Circle(new Point(0, 0), 3),
            new Circle(new Point(1, 1), 2),
            new Circle(new Point(2, 2), 1)
        };


        for (int i = 0; i < circles.Length - 1; i++)
        {
            for (int j = 0; j < circles.Length - i - 1; j++)
            {
                if (circles[j].Area > circles[j + 1].Area)
                {
                    
                    var temp = circles[j];
                    circles[j] = circles[j + 1];
                    circles[j + 1] = temp;
                }
            }
        }

        Console.WriteLine("\nCircles sorted by area:");
        foreach (var circle in circles)
        {
            Console.WriteLine(circle);
        }

        
        Circle smallestCircle = circles[0]; 
       

        Circle clonedCircle = (Circle)smallestCircle.Clone();
        Console.WriteLine("\nCloned smallest circle:");
        Console.WriteLine(clonedCircle);
    }
}