using System;
using System.Collections.Generic;
using System.Linq;

// Base class for all mail items
public abstract class MailItem
{
    public double Weight { get; protected set; }
    public string ShippingMethod { get; protected set; }
    public string DestinationAddress { get; protected set; }

    public MailItem(double weight, string shippingMethod, string destination)
    {
        Weight = weight;
        ShippingMethod = shippingMethod;
        DestinationAddress = destination;
    }

    public abstract double CalculatePostage();
}

// Letter class
public class Letter : MailItem
{
    public string Format { get; private set; } // "A3" or "A4"

    public Letter(double weight, string shippingMethod, string destination, string format)
        : base(weight, shippingMethod, destination)
    {
        Format = format;
    }

    public override double CalculatePostage()
    {
        double baseFare = (Format == "A4") ? 2.50 : 3.50;
        double postage = baseFare + 1.0 * (Weight / 1000); // Convert grams to kg
        if (ShippingMethod == "express")
        {
            postage *= 2.0;
        }
        return postage;
    }
}

// Parcel class
public class Parcel : MailItem
{
    public double Volume { get; private set; }

    public Parcel(double weight, string shippingMethod, string destination, double volume)
        : base(weight, shippingMethod, destination)
    {
        Volume = volume;
    }

    public override double CalculatePostage()
    {
        double postage = 0.25 * Volume + Weight / 1000; // Convert grams to kg
        if (ShippingMethod == "express")
        {
            postage *= 2.0;
        }
        return postage;
    }
}

// Advertisement class
public class Advertisement : MailItem
{
    public Advertisement(double weight, string shippingMethod, string destination)
        : base(weight, shippingMethod, destination)
    {
    }

    public override double CalculatePostage()
    {
        double postage = 5.0 * (Weight / 1000); // Convert grams to kg
        if (ShippingMethod == "express")
        {
            postage *= 2.0;
        }
        return postage;
    }
}

// Mailbox class
public class Mailbox
{
    private List<MailItem> items = new List<MailItem>();

    public void AddMailItem(MailItem item)
    {
        if (!string.IsNullOrEmpty(item.DestinationAddress))
        {
            items.Add(item);
        }
    }

    public double Stamp()
    {
        double totalPostage = 0.0;
        foreach (var item in items)
        {
            totalPostage += item.CalculatePostage();
        }
        return totalPostage;
    }

    public int InvalidMails()
    {
        return items.Count(item => string.IsNullOrEmpty(item.DestinationAddress));
    }

    public void Display()
    {
        Console.WriteLine("The total amount of postage is " + Stamp());
        foreach (var item in items)
        {
            Console.WriteLine(item.GetType().Name);
            if (string.IsNullOrEmpty(item.DestinationAddress))
            {
                Console.WriteLine("(Invalid courier)");
            }
            else
            {
                Console.WriteLine($"Weight: {item.Weight} grams");
                Console.WriteLine($"Express: {(item.ShippingMethod == "express" ? "yes" : "no")}");
                Console.WriteLine($"Destination: {item.DestinationAddress}");
                Console.WriteLine($"Price: ${item.CalculatePostage()}");
            }
        }
        Console.WriteLine("The box contains " + InvalidMails() + " invalid mails");
    }
}

class Program
{
    static void Main()
    {
        var mailbox = new Mailbox();
        mailbox.AddMailItem(new Letter(200, "express", "Toronto, 1009 Pully", "A3"));
        mailbox.AddMailItem(new Letter(800, "normal", "", "A4"));
        mailbox.AddMailItem(new Advertisement(1500, "express", "Muskoka, 1913 Saillon"));
        mailbox.AddMailItem(new Advertisement(3000, "normal", ""));
        mailbox.AddMailItem(new Parcel(5000, "express", "Vancouver, 1950 Sion", 85));
        mailbox.AddMailItem(new Parcel(3000, "express", "Montreal, 2800 Delemont", 100));

        mailbox.Display();
    }
}
