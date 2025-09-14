using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace MarioApp2025
{
    class DataCustomerInvoices
    {
        public void CustomerInvoicesInsertAndUpdate(string marLocatie, string sqlNew, string sqlUpdate, string cnn)
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

                    List<VsoftCustomerInvoice> customerInvoicesNew = new List<VsoftCustomerInvoice>();
                    List<VsoftCustomerInvoice> customerInvoicesUpdate = new List<VsoftCustomerInvoice>();


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

                        // decv055
                        decimal iodecv055;
                        if (!Convert.IsDBNull(readerNew["decv055"]))
                        {
                            iodecv055 = (decimal)readerNew["decv055"];
                        }
                        else
                        {
                            iodecv055 = 0;
                        }

                        // decv056
                        decimal iodecv056;
                        if (!Convert.IsDBNull(readerNew["decv056"]))
                        {
                            iodecv056 = (decimal)readerNew["decv056"];
                        }
                        else
                        {
                            iodecv056 = 0;
                        }

                        // decv057
                        decimal iodecv057;
                        if (!Convert.IsDBNull(readerNew["decv057"]))
                        {
                            iodecv057 = (decimal)readerNew["decv057"];
                        }
                        else
                        {
                            iodecv057 = 0;
                        }

                        // decv058
                        decimal iodecv058;
                        if (!Convert.IsDBNull(readerNew["decv058"]))
                        {
                            iodecv058 = (decimal)readerNew["decv058"];
                        }
                        else
                        {
                            iodecv058 = 0;
                        }

                        // decv059
                        decimal iodecv059;
                        if (!Convert.IsDBNull(readerNew["decv059"]))
                        {
                            iodecv059 = (decimal)readerNew["decv059"];
                        }
                        else
                        {
                            iodecv059 = 0;
                        }

                        // decv060
                        decimal iodecv060;
                        if (!Convert.IsDBNull(readerNew["decv060"]))
                        {
                            iodecv060 = (decimal)readerNew["decv060"];
                        }
                        else
                        {
                            iodecv060 = 0;
                        }

                        // decv061
                        decimal iodecv061;
                        if (!Convert.IsDBNull(readerNew["decv061"]))
                        {
                            iodecv061 = (decimal)readerNew["decv061"];
                        }
                        else
                        {
                            iodecv061 = 0;
                        }

                        // decv062
                        decimal iodecv062;
                        if (!Convert.IsDBNull(readerNew["decv062"]))
                        {
                            iodecv062 = (decimal)readerNew["decv062"];
                        }
                        else
                        {
                            iodecv062 = 0;
                        }

                        // decv063
                        decimal iodecv063;
                        if (!Convert.IsDBNull(readerNew["decv063"]))
                        {
                            iodecv063 = (decimal)readerNew["decv063"];
                        }
                        else
                        {
                            iodecv063 = 0;
                        }

                        // decv064
                        decimal iodecv064;
                        if (!Convert.IsDBNull(readerNew["decv064"]))
                        {
                            iodecv064 = (decimal)readerNew["decv064"];
                        }
                        else
                        {
                            iodecv064 = 0;
                        }

                        // decB010
                        decimal iodecB010;
                        if (!Convert.IsDBNull(readerNew["decB010"]))
                        {
                            iodecB010 = (decimal)readerNew["decB010"];
                        }
                        else
                        {
                            iodecB010 = 0;
                        }

                        // decB014
                        decimal iodecB014;
                        if (!Convert.IsDBNull(readerNew["decB014"]))
                        {
                            iodecB014 = (decimal)readerNew["decB014"];
                        }
                        else
                        {
                            iodecB014 = 0;
                        }

                        // decB090
                        decimal iodecB090;
                        if (!Convert.IsDBNull(readerNew["decB090"]))
                        {
                            iodecB090 = (decimal)readerNew["decB090"];
                        }
                        else
                        {
                            iodecB090 = 0;
                        }

                        // decB094
                        decimal iodecB094;
                        if (!Convert.IsDBNull(readerNew["decB094"]))
                        {
                            iodecB094 = (decimal)readerNew["decB094"];
                        }
                        else
                        {
                            iodecB094 = 0;
                        }

                        // decv065
                        decimal iodecv065;
                        if (!Convert.IsDBNull(readerNew["decv065"]))
                        {
                            iodecv065 = (decimal)readerNew["decv065"];
                        }
                        else
                        {
                            iodecv065 = 0;
                        }

                        customerInvoicesNew.Add(new VsoftCustomerInvoice
                        {
                            V033 = readerNew["v033"].ToString().Trim(' '),
                            A110 = readerNew["v034"].ToString().Substring(1).Trim(' '), // A110 Unique code number (Customer ID)
                            V035 = readerNew["v035"].ToString().Trim(' '),
                            V036 = readerNew["v036"].ToString().Trim(' '),
                            V037 = readerNew["v037"].ToString().Trim(' '),
                            V038 = readerNew["v038"].ToString().Trim(' '),
                            V039 = readerNew["v039"].ToString().Trim(' '),
                            Vs03 = readerNew["vs03"].ToString().Trim(' '),
                            V040 = readerNew["v040"].ToString().Trim(' '),

                            // Opgelet v041 is veld van slechts één teken, LinqToSql generator 2008 geeft CHAR weer welke geen string aanvaardt. 
                            V041 = readerNew["v041"].ToString().Trim(' '),
                            V055 = readerNew["v055"].ToString().Trim(' '),
                            V056 = readerNew["v056"].ToString().Trim(' '),
                            V057 = readerNew["v057"].ToString().Trim(' '),
                            V058 = readerNew["v058"].ToString().Trim(' '),
                            V059 = readerNew["v059"].ToString().Trim(' '),
                            V060 = readerNew["v060"].ToString().Trim(' '),
                            V061 = readerNew["v061"].ToString().Trim(' '),
                            V062 = readerNew["v062"].ToString().Trim(' '),
                            V063 = readerNew["v063"].ToString().Trim(' '),
                            V064 = readerNew["v064"].ToString().Trim(' '),
                            A000 = readerNew["A000"].ToString().Trim(' '),
                            B010 = readerNew["B010"].ToString().Trim(' '),
                            B014 = readerNew["B014"].ToString().Trim(' '),
                            B090 = readerNew["B090"].ToString().Trim(' '),
                            B094 = readerNew["B094"].ToString().Trim(' '),
                            V065 = readerNew["v065"].ToString().Trim(' '),
                            V245 = readerNew["v245"].ToString().Trim(' '),
                            V246 = readerNew["v246"].ToString().Trim(' '),
                            V066 = readerNew["v066"].ToString().Trim(' '),
                            V249 = readerNew["v249"].ToString().Trim(' '),

                            Decv249 = iodecv249,
                            Decv040 = iodecv040,
                            Decv055 = iodecv055,
                            Decv056 = iodecv056,
                            Decv057 = iodecv057,
                            Decv058 = iodecv058,
                            Decv059 = iodecv059,
                            Decv060 = iodecv060,
                            Decv061 = iodecv061,
                            Decv062 = iodecv062,
                            Decv063 = iodecv063,
                            Decv064 = iodecv064,
                            DecB010 = iodecB010,
                            DecB014 = iodecB014,
                            DecB090 = iodecB090,
                            DecB094 = iodecB094,
                            Decv065 = iodecv065,

                            // opgelet MEMO Field
                            Dece069 = readerNew["dece069"].ToString().Trim(' '),

                            E069 = readerNew["e069"].ToString().Trim(' '),
                            RvDm = readerNew["rvDM"].ToString().Trim(' '),
                            V405 = readerNew["v405"].ToString().Trim(' '),
                            V406 = readerNew["v406"].ToString().Trim(' '),
                            // BstndNaam37 = readerNew["bstndNaam37"].ToString().Trim(' '),
                            // TypeZending37 = readerNew["typeZending37"].ToString().Trim(' '),
                            // If IsDBNull(adoRS.Fields("bstBLOB37").Value) Then
                            // Else
                            // 'Stop
                            // '.bstBLOB37 = System.Convert.ToString(adoRS.Fields("bstBLOB37").Value)
                            // End If

                            RvXmltb2 = readerNew["rvXMLTB2"].ToString().Trim(' '),

                            // RvID = (int)readerNew["RvID"],
                            VsoftCustomerId = (int)readerNew["vsoftKLID"]
                        });
                    }

                    try
                    {
                        // connection.Execute("dbo.spVsoftCustomers_InsertFull @Id, @A107, @A108, @A100", customers);                    
                        connection.Execute("spVsoftCustomerInvoices_InsertFull @V033, @A110, @V035, @V066, @V036, @V037, @V038, @V249, @Decv249, @V039, @Vs03, @V040, @Decv040, @V041, @V245, @V246, @A000, @B010, @DecB010, @B014, @DecB014, @B090, @DecB090, @B094, @DecB094, @V065, @Decv065, @E069, @Dece069, @E071, @E072, @V055, @Decv055, @V056, @Decv056, @V057, @Decv057, @V058, @Decv058, @V059, @Decv059, @V060, @Decv060, @V061, @Decv061, @V062, @Decv062, @V063, @Decv063, @V064, @Decv064, @RvDm, @RvXmltb2, @V405, @V406, @VsoftCustomerId", customerInvoicesNew);
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

                        // decv055
                        decimal iodecv055;
                        if (!Convert.IsDBNull(readerUpdate["decv055"]))
                        {
                            iodecv055 = (decimal)readerUpdate["decv055"];
                        }
                        else
                        {
                            iodecv055 = 0;
                        }

                        // decv056
                        decimal iodecv056;
                        if (!Convert.IsDBNull(readerUpdate["decv056"]))
                        {
                            iodecv056 = (decimal)readerUpdate["decv056"];
                        }
                        else
                        {
                            iodecv056 = 0;
                        }

                        // decv057
                        decimal iodecv057;
                        if (!Convert.IsDBNull(readerUpdate["decv057"]))
                        {
                            iodecv057 = (decimal)readerUpdate["decv057"];
                        }
                        else
                        {
                            iodecv057 = 0;
                        }

                        // decv058
                        decimal iodecv058;
                        if (!Convert.IsDBNull(readerUpdate["decv058"]))
                        {
                            iodecv058 = (decimal)readerUpdate["decv058"];
                        }
                        else
                        {
                            iodecv058 = 0;
                        }

                        // decv059
                        decimal iodecv059;
                        if (!Convert.IsDBNull(readerUpdate["decv059"]))
                        {
                            iodecv059 = (decimal)readerUpdate["decv059"];
                        }
                        else
                        {
                            iodecv059 = 0;
                        }

                        // decv060
                        decimal iodecv060;
                        if (!Convert.IsDBNull(readerUpdate["decv060"]))
                        {
                            iodecv060 = (decimal)readerUpdate["decv060"];
                        }
                        else
                        {
                            iodecv060 = 0;
                        }

                        // decv061
                        decimal iodecv061;
                        if (!Convert.IsDBNull(readerUpdate["decv061"]))
                        {
                            iodecv061 = (decimal)readerUpdate["decv061"];
                        }
                        else
                        {
                            iodecv061 = 0;
                        }

                        // decv062
                        decimal iodecv062;
                        if (!Convert.IsDBNull(readerUpdate["decv062"]))
                        {
                            iodecv062 = (decimal)readerUpdate["decv062"];
                        }
                        else
                        {
                            iodecv062 = 0;
                        }

                        // decv063
                        decimal iodecv063;
                        if (!Convert.IsDBNull(readerUpdate["decv063"]))
                        {
                            iodecv063 = (decimal)readerUpdate["decv063"];
                        }
                        else
                        {
                            iodecv063 = 0;
                        }

                        // decv064
                        decimal iodecv064;
                        if (!Convert.IsDBNull(readerUpdate["decv064"]))
                        {
                            iodecv064 = (decimal)readerUpdate["decv064"];
                        }
                        else
                        {
                            iodecv064 = 0;
                        }

                        // decB010
                        decimal iodecB010;
                        if (!Convert.IsDBNull(readerUpdate["decB010"]))
                        {
                            iodecB010 = (decimal)readerUpdate["decB010"];
                        }
                        else
                        {
                            iodecB010 = 0;
                        }

                        // decB014
                        decimal iodecB014;
                        if (!Convert.IsDBNull(readerUpdate["decB014"]))
                        {
                            iodecB014 = (decimal)readerUpdate["decB014"];
                        }
                        else
                        {
                            iodecB014 = 0;
                        }

                        // decB090
                        decimal iodecB090;
                        if (!Convert.IsDBNull(readerUpdate["decB090"]))
                        {
                            iodecB090 = (decimal)readerUpdate["decB090"];
                        }
                        else
                        {
                            iodecB090 = 0;
                        }

                        // decB094
                        decimal iodecB094;
                        if (!Convert.IsDBNull(readerUpdate["decB094"]))
                        {
                            iodecB094 = (decimal)readerUpdate["decB094"];
                        }
                        else
                        {
                            iodecB094 = 0;
                        }

                        // decv065
                        decimal iodecv065;
                        if (!Convert.IsDBNull(readerUpdate["decv065"]))
                        {
                            iodecv065 = (decimal)readerUpdate["decv065"];
                        }
                        else
                        {
                            iodecv065 = 0;
                        }

                        customerInvoicesUpdate.Add(new VsoftCustomerInvoice
                        {
                            A110 = readerUpdate["v034"].ToString().Substring(1).Trim(' '), // A110 Unique code number (Customer ID)
                            V033 = readerUpdate["v033"].ToString().Trim(' '),
                            V035 = readerUpdate["v035"].ToString().Trim(' '),
                            V036 = readerUpdate["v036"].ToString().Trim(' '),
                            V037 = readerUpdate["v037"].ToString().Trim(' '),
                            V038 = readerUpdate["v038"].ToString().Trim(' '),
                            V039 = readerUpdate["v039"].ToString().Trim(' '),
                            Vs03 = readerUpdate["vs03"].ToString().Trim(' '),
                            V040 = readerUpdate["v040"].ToString().Trim(' '),

                            // Opgelet v041 is veld van slechts één teken, LinqToSql generator 2008 geeft CHAR weer welke geen string aanvaardt. 
                            V041 = readerUpdate["v041"].ToString().Trim(' '),
                            V055 = readerUpdate["v055"].ToString().Trim(' '),
                            V056 = readerUpdate["v056"].ToString().Trim(' '),
                            V057 = readerUpdate["v057"].ToString().Trim(' '),
                            V058 = readerUpdate["v058"].ToString().Trim(' '),
                            V059 = readerUpdate["v059"].ToString().Trim(' '),
                            V060 = readerUpdate["v060"].ToString().Trim(' '),
                            V061 = readerUpdate["v061"].ToString().Trim(' '),
                            V062 = readerUpdate["v062"].ToString().Trim(' '),
                            V063 = readerUpdate["v063"].ToString().Trim(' '),
                            V064 = readerUpdate["v064"].ToString().Trim(' '),
                            A000 = readerUpdate["A000"].ToString().Trim(' '),
                            B010 = readerUpdate["B010"].ToString().Trim(' '),
                            B014 = readerUpdate["B014"].ToString().Trim(' '),
                            B090 = readerUpdate["B090"].ToString().Trim(' '),
                            B094 = readerUpdate["B094"].ToString().Trim(' '),
                            V065 = readerUpdate["v065"].ToString().Trim(' '),
                            V245 = readerUpdate["v245"].ToString().Trim(' '),
                            V246 = readerUpdate["v246"].ToString().Trim(' '),
                            V066 = readerUpdate["v066"].ToString().Trim(' '),
                            V249 = readerUpdate["v249"].ToString().Trim(' '),

                            Decv249 = iodecv249,
                            Decv040 = iodecv040,
                            Decv055 = iodecv055,
                            Decv056 = iodecv056,
                            Decv057 = iodecv057,
                            Decv058 = iodecv058,
                            Decv059 = iodecv059,
                            Decv060 = iodecv060,
                            Decv061 = iodecv061,
                            Decv062 = iodecv062,
                            Decv063 = iodecv063,
                            Decv064 = iodecv064,
                            DecB010 = iodecB010,
                            DecB014 = iodecB014,
                            DecB090 = iodecB090,
                            DecB094 = iodecB094,
                            Decv065 = iodecv065,

                            // opgelet MEMO Field
                            Dece069 = readerUpdate["dece069"].ToString().Trim(' '),

                            E069 = readerUpdate["e069"].ToString().Trim(' '),
                            RvDm = readerUpdate["rvDM"].ToString().Trim(' '),
                            V405 = readerUpdate["v405"].ToString().Trim(' '),
                            V406 = readerUpdate["v406"].ToString().Trim(' '),
                            // BstndNaam37 = readerUpdate["bstndNaam37"].ToString().Trim(' '),
                            // TypeZending37 = readerUpdate["typeZending37"].ToString().Trim(' '),
                            // If IsDBNull(adoRS.Fields("bstBLOB37").Value) Then
                            // Else
                            // 'Stop
                            // '.bstBLOB37 = System.Convert.ToString(adoRS.Fields("bstBLOB37").Value)
                            // End If

                            RvXmltb2 = readerUpdate["rvXMLTB2"].ToString().Trim(' '),
                            // RvID = (int)readerUpdate["RvID"],
                            VsoftCustomerId = (int)readerUpdate["vsoftKLID"]
                        });
                    }

                    try
                    {
                        // connection.Execute("dbo.spVsoftCustomers_InsertFull @Id, @A107, @A108, @A100", customers);                    
                        connection.Execute("spVsoftCustomerInvoices_UpdateFull @V033, @A110, @V035, @V066, @V036, @V037, @V038, @V249, @Decv249, @V039, @Vs03, @V040, @Decv040, @V041, @V245, @V246, @A000, @B010, @DecB010, @B014, @DecB014, @B090, @DecB090, @B094, @DecB094, @V065, @Decv065, @E069, @Dece069, @E071, @E072, @V055, @Decv055, @V056, @Decv056, @V057, @Decv057, @V058, @Decv058, @V059, @Decv059, @V060, @Decv060, @V061, @Decv061, @V062, @Decv062, @V063, @Decv063, @V064, @Decv064, @RvDm, @RvXmltb2, @V405, @V406, @VsoftCustomerId", customerInvoicesUpdate);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    readerUpdate.Close();

                    OleDbCommand command = new OleDbCommand("UPDATE Dokumenten SET dnnSync=True, dnnSyncAlready = True where v034 like 'K%'", cnnMarJet);
                    command.ExecuteNonQuery();
                    cnnMarJet.Close();
                }
                catch (Exception)
                {
                    throw;
                }

            }

        }

        public void CustomerInvoicesSyncID(string marLocatie, string cnn)
        {
            // Load records from SQL Server into a DataTable
            DataTable sqlTable = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(cnn))
            {
                sqlConn.Open();
                string sqlQuery = "SELECT [Id], [V033] FROM VsoftCustomerInvoices"; // adjust column names accordingly
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
