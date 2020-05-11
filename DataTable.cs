using System;
using System.Data;

class Program
{
    static void Main()
    {
      
        DataTable orderTable = CreateOrderTable();
        DataTable orderDetailTable = CreateOrderDetailTable();
        DataSet salesSet = new DataSet();
        salesSet.Tables.Add(orderTable);
        salesSet.Tables.Add(orderDetailTable);

        salesSet.Relations.Add("OrderOrderDetail", orderTable.Columns["OrderId"], orderDetailTable.Columns["OrderId"], true);

        try
        {
            DataRow errorRow = orderDetailTable.NewRow();
            errorRow[0] = 1;
            errorRow[1] = "O0009";
            orderDetailTable.Rows.Add(errorRow);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        Console.WriteLine();
      
        InsertOrders(orderTable);
        InsertOrderDetails(orderDetailTable);

        Console.WriteLine("The initial Order table.");
        ShowTable(orderTable);

        Console.WriteLine("The OrderDetail table.");
        ShowTable(orderDetailTable);
       
        DataColumn colSub = new DataColumn("SubTotal", typeof(Decimal), "Sum(Child.LineTotal)");
        orderTable.Columns.Add(colSub);
      
        DataColumn colTax = new DataColumn("Tax", typeof(Decimal), "SubTotal*0.1");
        orderTable.Columns.Add(colTax);
       
        DataColumn colTotal = new DataColumn("TotalDue", typeof(Decimal), "IIF(OrderId='Total',Sum(SubTotal)+Sum(Tax),SubTotal+Tax)");
        orderTable.Columns.Add(colTotal);

        DataRow row = orderTable.NewRow();
        row["OrderId"] = "Total";
        orderTable.Rows.Add(row);

        Console.WriteLine("The Order table with the expression columns.");
        ShowTable(orderTable);
       
        Console.ReadKey();
    }
    private static DataTable CreateOrderTable()
    {
        DataTable orderTable = new DataTable("Order");
       
        DataColumn colId = new DataColumn("OrderId", typeof(String));
        orderTable.Columns.Add(colId);

        DataColumn colDate = new DataColumn("OrderDate", typeof(DateTime));
        orderTable.Columns.Add(colDate);
      
        orderTable.PrimaryKey = new DataColumn[] { colId };

        return orderTable;
    }
    private static DataTable CreateOrderDetailTable()
    {
        DataTable orderDetailTable = new DataTable("OrderDetail");
      
        DataColumn[] cols ={
                                  new DataColumn("OrderDetailId",typeof(Int32)),
                                  new DataColumn("OrderId",typeof(String)),
                                  new DataColumn("Product",typeof(String)),
                                  new DataColumn("UnitPrice",typeof(Decimal)),
                                  new DataColumn("OrderQty",typeof(Int32)),
                                  new DataColumn("LineTotal",typeof(Decimal),"UnitPrice*OrderQty")
                              };

        orderDetailTable.Columns.AddRange(cols);
        orderDetailTable.PrimaryKey = new DataColumn[] { orderDetailTable.Columns["OrderDetailId"] };
        return orderDetailTable;
    }
    private static void InsertOrders(DataTable orderTable)
    {
        
        DataRow row1 = orderTable.NewRow();
        row1["OrderId"] = "O0001";
        row1["OrderDate"] = new DateTime(2020, 3, 1);
        orderTable.Rows.Add(row1);

        DataRow row2 = orderTable.NewRow();
        row2["OrderId"] = "O0002";
        row2["OrderDate"] = new DateTime(2020, 3, 12);
        orderTable.Rows.Add(row2);

        DataRow row3 = orderTable.NewRow();
        row3["OrderId"] = "O0003";
        row3["OrderDate"] = new DateTime(2020, 3, 20);
        orderTable.Rows.Add(row3);
    }
    private static void InsertOrderDetails(DataTable orderDetailTable)
    {
        
        Object[] rows = {
                                 new Object[]{1,"O0001","Car1",1419.5,10},
                                 new Object[]{2,"O0001","Car2",1233.6,20},
                                 new Object[]{3,"O0001","Car3",1653.3,30},
                                 new Object[]{4,"O0002","Car4",1419.5,40},
                                 new Object[]{5,"O0002","Car5",1233.6,50},
                                 new Object[]{6,"O0003","Car6",1419.5,60},
                                 new Object[]{7,"O0003","Car7",1653.3,70},
                             };

        foreach (Object[] row in rows)
        {
            orderDetailTable.Rows.Add(row);
        }
    }
    private static void ShowTable(DataTable table)
    {
        foreach (DataColumn col in table.Columns)
        {
            Console.Write("{0,-14}", col.ColumnName);
        }
        Console.WriteLine();

        foreach (DataRow row in table.Rows)
        {
            foreach (DataColumn col in table.Columns)
            {
                if (col.DataType.Equals(typeof(DateTime)))
                    Console.Write("{0,-14:d}", row[col]);
                else if (col.DataType.Equals(typeof(Decimal)))
                    Console.Write("{0,-14:C}", row[col]);
                else
                    Console.Write("{0,-14}", row[col]);
            }
            Console.WriteLine();
        }
        Console.WriteLine("-By Denis Rafi");
    }
}