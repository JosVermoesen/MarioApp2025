using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace MarioApp2025
{
    class DataSupplierInvoices
    {
        public void SupplierInvoicesInsertAndUpdate(string marLocatie, string sqlNew, string sqlUpdate, string cnn)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(cnn))
            {
                try
                {
                    OleDbConnection cnnMarJet = new OleDbConnection
                    {
                        ConnectionString = marLocatie
                    };

                    cnnMarJet.Open();

                    OleDbCommand cmdNew = new OleDbCommand
                    {
                        Connection = cnnMarJet,
                        CommandText = sqlNew
                    };

                    OleDbCommand cmdUpdate = new OleDbCommand
                    {
                        Connection = cnnMarJet,
                        CommandText = sqlUpdate
                    };


                    OleDbDataReader readerNew = cmdNew.ExecuteReader();
                    OleDbDataReader readerUpdate = cmdUpdate.ExecuteReader();

                    List<VsoftSupplierInvoice> supplierInvoicesNew = new List<VsoftSupplierInvoice>();
                    List<VsoftSupplierInvoice> supplierInvoicesUpdate = new List<VsoftSupplierInvoice>();

                    while (readerNew.Read())
                    {
                        // decv249
                        decimal iodecv249;
                        if (!Convert.IsDBNull(readerNew["decv249"]))
                        {
                            iodecv249 = (decimal)readerNew["decv249"];
                        }
                        else
                        {
                            iodecv249 = 0;
                        }

                        // decv040
                        decimal iodecv040;
                        if (!Convert.IsDBNull(readerNew["decv040"]))
                        {
                            iodecv040 = (decimal)readerNew["decv040"];
                        }
                        else
                        {
                            iodecv040 = 0;
                        }

                        // decv042
                        decimal iodecv042;
                        if (!Convert.IsDBNull(readerNew["decv042"]))
                        {
                            iodecv042 = (decimal)readerNew["decv042"];
                        }
                        else
                        {
                            iodecv042 = 0;
                        }

                        // decv043
                        decimal iodecv043;
                        if (!Convert.IsDBNull(readerNew["decv043"]))
                        {
                            iodecv043 = (decimal)readerNew["decv043"];
                        }
                        else
                        {
                            iodecv043 = 0;
                        }
                        // decv044
                        decimal iodecv044;
                        if (!Convert.IsDBNull(readerNew["decv044"]))
                        {
                            iodecv044 = (decimal)readerNew["decv044"];
                        }
                        else
                        {
                            iodecv044 = 0;
                        }
                        // decv045
                        decimal iodecv045;
                        if (!Convert.IsDBNull(readerNew["decv045"]))
                        {
                            iodecv045 = (decimal)readerNew["decv045"];
                        }
                        else
                        {
                            iodecv045 = 0;
                        }
                        // decv046
                        decimal iodecv046;
                        if (!Convert.IsDBNull(readerNew["decv046"]))
                        {
                            iodecv046 = (decimal)readerNew["decv046"];
                        }
                        else
                        {
                            iodecv046 = 0;
                        }
                        // decv047
                        decimal iodecv047;
                        if (!Convert.IsDBNull(readerNew["decv047"]))
                        {
                            iodecv047 = (decimal)readerNew["decv047"];
                        }
                        else
                        {
                            iodecv047 = 0;
                        }
                        // decv048
                        decimal iodecv048;
                        if (!Convert.IsDBNull(readerNew["decv048"]))
                        {
                            iodecv048 = (decimal)readerNew["decv048"];
                        }
                        else
                        {
                            iodecv048 = 0;
                        }
                        // decv049
                        decimal iodecv049;
                        if (!Convert.IsDBNull(readerNew["decv049"]))
                        {
                            iodecv049 = (decimal)readerNew["decv049"];
                        }
                        else
                        {
                            iodecv049 = 0;
                        }
                        // decv050
                        decimal iodecv050;
                        if (!Convert.IsDBNull(readerNew["decv050"]))
                        {
                            iodecv050 = (decimal)readerNew["decv050"];
                        }
                        else
                        {
                            iodecv050 = 0;
                        }
                        // decv051
                        decimal iodecv051;
                        if (!Convert.IsDBNull(readerNew["decv051"]))
                        {
                            iodecv051 = (decimal)readerNew["decv051"];
                        }
                        else
                        {
                            iodecv051 = 0;
                        }
                        // decv052
                        decimal iodecv052;
                        if (!Convert.IsDBNull(readerNew["decv052"]))
                        {
                            iodecv052 = (decimal)readerNew["decv052"];
                        }
                        else
                        {
                            iodecv052 = 0;
                        }
                        // decv053
                        decimal iodecv053;
                        if (!Convert.IsDBNull(readerNew["decv053"]))
                        {
                            iodecv053 = (decimal)readerNew["decv053"];
                        }
                        else
                        {
                            iodecv053 = 0;
                        }
                        // decv054
                        decimal iodecv054;
                        if (!Convert.IsDBNull(readerNew["decv054"]))
                        {
                            iodecv054 = (decimal)readerNew["decv054"];
                        }
                        else
                        {
                            iodecv054 = 0;
                        }

                        supplierInvoicesNew.Add(new VsoftSupplierInvoice
                        {
                            A110 = readerNew["v034"].ToString().Substring(1).Trim(' '),
                            V033 = readerNew["v033"].ToString().Trim(' '),
                            V035 = readerNew["v035"].ToString().Trim(' '),
                            V066 = readerNew["v066"].ToString().Trim(' '),
                            V036 = readerNew["v036"].ToString().Trim(' '),
                            V037 = readerNew["v037"].ToString().Trim(' '),
                            V038 = readerNew["v038"].ToString().Trim(' '),
                            V249 = readerNew["v249"].ToString().Trim(' '),

                            Decv249 = iodecv249,
                            Decv040 = iodecv040,

                            V039 = readerNew["v039"].ToString().Trim(' '),
                            Vs03 = readerNew["vs03"].ToString().Trim(' '),
                            V040 = readerNew["v040"].ToString().Trim(' '),

                            // Opgelet v041 is veld van slechts één teken, LinqToSql generator 2008 geeft CHAR weer welke geen string aanvaardt. 
                            V041 = readerNew["v041"].ToString().Trim(' '),
                            V245 = readerNew["v245"].ToString().Trim(' '),
                            V246 = readerNew["v246"].ToString().Trim(' '),

                            RvDm = readerNew["rvDM"].ToString().Trim(' '),

                            // BstndNaam37 = readerNew["bstndNaam37"].ToString().Trim(' '),
                            // TypeZending37 = readerNew["typeZending37"].ToString().Trim(' '),
                            // If IsDBNull(adoRS.Fields("bstBLOB37").Value) Then
                            // Else
                            // 'Stop
                            // '.bstBLOB37 = System.Convert.ToString(adoRS.Fields("bstBLOB37").Value)
                            // End If

                            V042 = readerNew["v042"].ToString().Trim(' '),
                            V043 = readerNew["v043"].ToString().Trim(' '),
                            V044 = readerNew["v044"].ToString().Trim(' '),
                            V045 = readerNew["v045"].ToString().Trim(' '),
                            V046 = readerNew["v046"].ToString().Trim(' '),
                            V047 = readerNew["v047"].ToString().Trim(' '),
                            V048 = readerNew["v048"].ToString().Trim(' '),
                            V049 = readerNew["v049"].ToString().Trim(' '),
                            V050 = readerNew["v050"].ToString().Trim(' '),
                            V051 = readerNew["v051"].ToString().Trim(' '),
                            V052 = readerNew["v052"].ToString().Trim(' '),
                            V053 = readerNew["v053"].ToString().Trim(' '),
                            V054 = readerNew["v054"].ToString().Trim(' '),

                            Decv042 = iodecv042,
                            Decv043 = iodecv043,
                            Decv044 = iodecv044,
                            Decv045 = iodecv045,
                            Decv046 = iodecv046,
                            Decv047 = iodecv047,
                            Decv048 = iodecv048,
                            Decv049 = iodecv049,
                            Decv050 = iodecv050,
                            Decv051 = iodecv051,
                            Decv052 = iodecv052,
                            Decv053 = iodecv053,
                            Decv054 = iodecv054,

                            // RvID = (int)readerNew["RvID"],
                            VsoftSupplierId = (int)readerNew["vsoftKLID"]
                        });
                    }

                    try
                    {
                        connection.Execute("spVsoftSupplierInvoices_InsertFull @V033, @A110, @V035, @V066, @V036, @V037, @V038, @V249, @Decv249, @V039, @Vs03, @V040, @Decv040, @V041, @V245, @V246, @RvDm, @V042, @V043, @V044, @V045, @V046, @V047, @V048, @V049, @V050, @V051, @V052, @V053, @V054, @Decv042, @Decv043, @Decv044, @Decv045, @Decv046, @Decv047, @Decv048, @Decv049, @Decv050, @Decv051, @Decv052, @Decv053, @Decv054, @VsoftSupplierId", supplierInvoicesNew);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    readerNew.Close();


                    while (readerUpdate.Read())
                    {
                        // decv249
                        decimal iodecv249;
                        if (!Convert.IsDBNull(readerUpdate["decv249"]))
                        {
                            iodecv249 = (decimal)readerUpdate["decv249"];
                        }
                        else
                        {
                            iodecv249 = 0;
                        }

                        // decv040
                        decimal iodecv040;
                        if (!Convert.IsDBNull(readerUpdate["decv040"]))
                        {
                            iodecv040 = (decimal)readerUpdate["decv040"];
                        }
                        else
                        {
                            iodecv040 = 0;
                        }

                        // decv042
                        decimal iodecv042;
                        if (!Convert.IsDBNull(readerUpdate["decv042"]))
                        {
                            iodecv042 = (decimal)readerUpdate["decv042"];
                        }
                        else
                        {
                            iodecv042 = 0;
                        }

                        // decv043
                        decimal iodecv043;
                        if (!Convert.IsDBNull(readerUpdate["decv043"]))
                        {
                            iodecv043 = (decimal)readerUpdate["decv043"];
                        }
                        else
                        {
                            iodecv043 = 0;
                        }
                        // decv044
                        decimal iodecv044;
                        if (!Convert.IsDBNull(readerUpdate["decv044"]))
                        {
                            iodecv044 = (decimal)readerUpdate["decv044"];
                        }
                        else
                        {
                            iodecv044 = 0;
                        }
                        // decv045
                        decimal iodecv045;
                        if (!Convert.IsDBNull(readerUpdate["decv045"]))
                        {
                            iodecv045 = (decimal)readerUpdate["decv045"];
                        }
                        else
                        {
                            iodecv045 = 0;
                        }
                        // decv046
                        decimal iodecv046;
                        if (!Convert.IsDBNull(readerUpdate["decv046"]))
                        {
                            iodecv046 = (decimal)readerUpdate["decv046"];
                        }
                        else
                        {
                            iodecv046 = 0;
                        }
                        // decv047
                        decimal iodecv047;
                        if (!Convert.IsDBNull(readerUpdate["decv047"]))
                        {
                            iodecv047 = (decimal)readerUpdate["decv047"];
                        }
                        else
                        {
                            iodecv047 = 0;
                        }
                        // decv048
                        decimal iodecv048;
                        if (!Convert.IsDBNull(readerUpdate["decv048"]))
                        {
                            iodecv048 = (decimal)readerUpdate["decv048"];
                        }
                        else
                        {
                            iodecv048 = 0;
                        }
                        // decv049
                        decimal iodecv049;
                        if (!Convert.IsDBNull(readerUpdate["decv049"]))
                        {
                            iodecv049 = (decimal)readerUpdate["decv049"];
                        }
                        else
                        {
                            iodecv049 = 0;
                        }
                        // decv050
                        decimal iodecv050;
                        if (!Convert.IsDBNull(readerUpdate["decv050"]))
                        {
                            iodecv050 = (decimal)readerUpdate["decv050"];
                        }
                        else
                        {
                            iodecv050 = 0;
                        }
                        // decv051
                        decimal iodecv051;
                        if (!Convert.IsDBNull(readerUpdate["decv051"]))
                        {
                            iodecv051 = (decimal)readerUpdate["decv051"];
                        }
                        else
                        {
                            iodecv051 = 0;
                        }
                        // decv052
                        decimal iodecv052;
                        if (!Convert.IsDBNull(readerUpdate["decv052"]))
                        {
                            iodecv052 = (decimal)readerUpdate["decv052"];
                        }
                        else
                        {
                            iodecv052 = 0;
                        }
                        // decv053
                        decimal iodecv053;
                        if (!Convert.IsDBNull(readerUpdate["decv053"]))
                        {
                            iodecv053 = (decimal)readerUpdate["decv053"];
                        }
                        else
                        {
                            iodecv053 = 0;
                        }
                        // decv054
                        decimal iodecv054;
                        if (!Convert.IsDBNull(readerUpdate["decv054"]))
                        {
                            iodecv054 = (decimal)readerUpdate["decv054"];
                        }
                        else
                        {
                            iodecv054 = 0;
                        }

                        supplierInvoicesUpdate.Add(new VsoftSupplierInvoice
                        {
                            A110 = readerUpdate["v034"].ToString().Substring(1).Trim(' '),
                            V033 = readerUpdate["v033"].ToString().Trim(' '),
                            V035 = readerUpdate["v035"].ToString().Trim(' '),
                            V066 = readerUpdate["v066"].ToString().Trim(' '),
                            V036 = readerUpdate["v036"].ToString().Trim(' '),
                            V037 = readerUpdate["v037"].ToString().Trim(' '),
                            V038 = readerUpdate["v038"].ToString().Trim(' '),
                            V249 = readerUpdate["v249"].ToString().Trim(' '),

                            Decv249 = iodecv249,
                            Decv040 = iodecv040,

                            V039 = readerUpdate["v039"].ToString().Trim(' '),
                            Vs03 = readerUpdate["vs03"].ToString().Trim(' '),
                            V040 = readerUpdate["v040"].ToString().Trim(' '),

                            // Opgelet v041 is veld van slechts één teken, LinqToSql generator 2008 geeft CHAR weer welke geen string aanvaardt. 
                            V041 = readerUpdate["v041"].ToString().Trim(' '),
                            V245 = readerUpdate["v245"].ToString().Trim(' '),
                            V246 = readerUpdate["v246"].ToString().Trim(' '),

                            RvDm = readerUpdate["rvDM"].ToString().Trim(' '),

                            // BstndNaam37 = readerUpdate["bstndNaam37"].ToString().Trim(' '),
                            // TypeZending37 = readerUpdate["typeZending37"].ToString().Trim(' '),
                            // If IsDBNull(adoRS.Fields("bstBLOB37").Value) Then
                            // Else
                            // 'Stop
                            // '.bstBLOB37 = System.Convert.ToString(adoRS.Fields("bstBLOB37").Value)
                            // End If

                            V042 = readerUpdate["v042"].ToString().Trim(' '),
                            V043 = readerUpdate["v043"].ToString().Trim(' '),
                            V044 = readerUpdate["v044"].ToString().Trim(' '),
                            V045 = readerUpdate["v045"].ToString().Trim(' '),
                            V046 = readerUpdate["v046"].ToString().Trim(' '),
                            V047 = readerUpdate["v047"].ToString().Trim(' '),
                            V048 = readerUpdate["v048"].ToString().Trim(' '),
                            V049 = readerUpdate["v049"].ToString().Trim(' '),
                            V050 = readerUpdate["v050"].ToString().Trim(' '),
                            V051 = readerUpdate["v051"].ToString().Trim(' '),
                            V052 = readerUpdate["v052"].ToString().Trim(' '),
                            V053 = readerUpdate["v053"].ToString().Trim(' '),
                            V054 = readerUpdate["v054"].ToString().Trim(' '),

                            Decv042 = iodecv042,
                            Decv043 = iodecv043,
                            Decv044 = iodecv044,
                            Decv045 = iodecv045,
                            Decv046 = iodecv046,
                            Decv047 = iodecv047,
                            Decv048 = iodecv048,
                            Decv049 = iodecv049,
                            Decv050 = iodecv050,
                            Decv051 = iodecv051,
                            Decv052 = iodecv052,
                            Decv053 = iodecv053,
                            Decv054 = iodecv054,

                            // RvID = (int)readerUpdate["RvID"],
                            VsoftSupplierId = (int)readerUpdate["vsoftKLID"]
                        });
                    }

                    try
                    {
                        connection.Execute("spVsoftSupplierInvoices_UpdateFull @V033, @A110, @V035, @V066, @V036, @V037, @V038, @V249, @Decv249, @V039, @Vs03, @V040, @Decv040, @V041, @V245, @V246, @RvDm, @V042, @V043, @V044, @V045, @V046, @V047, @V048, @V049, @V050, @V051, @V052, @V053, @V054, @Decv042, @Decv043, @Decv044, @Decv045, @Decv046, @Decv047, @Decv048, @Decv049, @Decv050, @Decv051, @Decv052, @Decv053, @Decv054, @VsoftSupplierId", supplierInvoicesUpdate);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    readerUpdate.Close();

                    OleDbCommand command = new OleDbCommand("UPDATE Dokumenten SET dnnSync=True, dnnSyncAlready = True where v034 like 'L%'", cnnMarJet);
                    command.ExecuteNonQuery();
                    cnnMarJet.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public void SupplierInvoicesSyncID(string marLocatie, string cnn)
        {
            // Load records from SQL Server into a DataTable
            DataTable sqlTable = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(cnn))
            {
                sqlConn.Open();
                string sqlQuery = "SELECT [Id], [V033] FROM VsoftSupplierInvoices"; // adjust column names accordingly
                using (SqlCommand sqlCmd = new SqlCommand(sqlQuery, sqlConn))
                {
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd);
                    sqlAdapter.Fill(sqlTable);
                }
            }

            // Connect to Access and sync each record
            using (OleDbConnection accessConn = new OleDbConnection(marLocatie))
            {
                accessConn.Open();

                foreach (DataRow row in sqlTable.Rows)
                {
                    int sqlID = Convert.ToInt32(row["Id"]);
                    string V033 = (string)row["V033"]; // adjust as needed

                    // Check if this record already exists in the Access table
                    bool recordExists = false;
                    using (OleDbCommand checkCmd = new OleDbCommand("SELECT COUNT(*) FROM Dokumenten WHERE [V033] = ?", accessConn))
                    {
                        checkCmd.Parameters.AddWithValue("?", V033);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        recordExists = (count > 0);
                    }

                    if (!recordExists)
                    {
                        Console.WriteLine("Record niet gevonden in marIntegraal mdv tabel.");
                    }
                    else
                    {
                        // Update the record if other fields have changed
                        using (OleDbCommand updateCmd = new OleDbCommand("UPDATE Dokumenten SET sqlID = ? WHERE [V033] = ?", accessConn))
                        {
                            updateCmd.Parameters.AddWithValue("?", sqlID);
                            updateCmd.Parameters.AddWithValue("?", V033);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            Console.WriteLine("Database sync completed successfully.");
        }
    }
}
