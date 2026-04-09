using myproject.IMap;
using myproject1.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using YourProject.Implementations;
using YourProject1.Implementations;


namespace first_lab
{
    public partial class Form1 : Form
    {
        // Текущий экземпляр карты (dynamic позволяет работать с разными типами карт)
        private dynamic currentMap;

        // Типы по умолчанию для ключей и значений
        private Type keyType = typeof(string);
        private Type valueType = typeof(string);
        public Form1()
        {
            InitializeComponent();
            InitializeMapTypes();
            InitializeTypeSelection();
        }
        // Инициализация выпадающих списков для выбора типов ключей и значений
        private void InitializeTypeSelection()
        {
            // Добавляем поддерживаемые типы в комбо-боксы
            cmbKeyType.Items.AddRange(new object[] { "string", "int", "double", "bool" });
            cmbValueType.Items.AddRange(new object[] { "string", "int", "double", "bool" });
            cmbKeyType.SelectedIndex = 0;
            cmbValueType.SelectedIndex = 0;

            // Add event handlers for type changes
            // Обработчики событий при изменении выбора типа
            cmbKeyType.SelectedIndexChanged += (s, e) =>
            {
                keyType = GetTypeFromString(cmbKeyType.SelectedItem.ToString());
                txtKey.Text = "";
            };

            cmbValueType.SelectedIndexChanged += (s, e) =>
            {
                valueType = GetTypeFromString(cmbValueType.SelectedItem.ToString());
                txtValue.Text = "";
            };
        }
        // Преобразование строки в соответствующий тип
        private Type GetTypeFromString(string typeName)
        {
            return typeName switch
            {
                "string" => typeof(string),
                "int" => typeof(int),
                "double" => typeof(double),
                "bool" => typeof(bool),
                _ => typeof(string)
            };
        }
        private void InitializeMapTypes()
        {
            cmbMapType.Items.Add("ArrayMap");
            cmbMapType.Items.Add("LinkedMap");
            cmbMapType.Items.Add("HashMap");
            cmbMapType.SelectedIndex = 0; // Default selection
        }

        // Валидация введенных данных в соответствии с типом
        private bool ValidateInput(string input, Type targetType)
        {
            if (string.IsNullOrEmpty(input))
            {
                return targetType == typeof(string); // Only strings can be empty
            }

            try
            {
                switch (Type.GetTypeCode(targetType))
                {
                    case TypeCode.String:
                        return true; // Accepts any string input

                    case TypeCode.Int32:
                        return int.TryParse(input, out _); // Проверка на целое число

                    case TypeCode.Double:
                        return double.TryParse(input, out _);  // Проверка на число с плавающей точкой

                    case TypeCode.Boolean:
                        // Accept true/false (case insensitive) or 1/0
                        return input.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                               input.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                               input == "1" || input == "0";

                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void btnCreateMap_Click(object sender, EventArgs e)
        {
            lblCount.Visible = true;
           
            string selectedMapType = cmbMapType.SelectedItem.ToString();

            try
            {
                currentMap = CreateMapInstance(selectedMapType, keyType, valueType);
                btnAdd.Enabled = true;
                btnRemove.Enabled = true;

                MessageBox.Show($"{selectedMapType}<{keyType.Name}, {valueType.Name}> created successfully!");
                UpdateMapDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating map: {ex.Message}");
            }
        }

        // Создание экземпляра карты с помощью рефлексии
        private dynamic CreateMapInstance(string mapType, Type keyType, Type valueType)
        {
            if (!typeof(IComparable).IsAssignableFrom(keyType))
            {
                throw new Exception($"Key type {keyType.Name} must implement IComparable");
            }

            Type genericType = mapType switch
            {
                "ArrayMap" => typeof(ArrayMap<,>),
                "LinkedMap" => typeof(LinkedMap<,>),
                "HashMap" => typeof(HashMap<,>),
                _ => throw new Exception("Unknown map type")
            };

            Type concreteType = genericType.MakeGenericType(keyType, valueType);
            return Activator.CreateInstance(concreteType);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (currentMap == null)
            {
                MessageBox.Show("Please create a map first.");
                return;
            }

            // Validate key
            if (!ValidateInput(txtKey.Text, keyType))
            {
                MessageBox.Show($"Invalid key format for {keyType.Name} type");
                return;
            }

            // Validate value
            if (!ValidateInput(txtValue.Text, valueType))
            {
                MessageBox.Show($"Invalid value format for {valueType.Name} type");
                return;
            }

            try
            {
                dynamic key = ConvertInput(txtKey.Text, keyType);
                dynamic value = ConvertInput(txtValue.Text, valueType);

                currentMap.Put(key, value);
                UpdateMapDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding entry: {ex.Message}");
            }
        }

        // Конвертация строкового ввода в нужный тип
        private dynamic ConvertInput(string input, Type targetType)
        {
            if (string.IsNullOrEmpty(input))
            {
                if (targetType == typeof(string)) return input;
                throw new Exception($"Value cannot be empty for {targetType.Name}");
            }

            try
            {
                switch (Type.GetTypeCode(targetType))
                {
                    case TypeCode.String:
                        return input;

                    case TypeCode.Int32:
                        return int.Parse(input);

                    case TypeCode.Double:
                        return double.Parse(input);

                    case TypeCode.Boolean:
                        if (input.Equals("true", StringComparison.OrdinalIgnoreCase)) return true;
                        if (input.Equals("false", StringComparison.OrdinalIgnoreCase)) return false;
                        if (input == "1") return true;
                        if (input == "0") return false;
                        throw new Exception("Must be true/false or 1/0");

                    default:
                        throw new Exception($"Unsupported type: {targetType.Name}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Invalid {targetType.Name} value: {ex.Message}");
            }
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (currentMap == null)
            {
                MessageBox.Show("Please create a map first.");
                return;
            }

            if (!ValidateInput(txtKey.Text, keyType))
            {
                MessageBox.Show($"Invalid key format for {keyType.Name} type");
                return;
            }

            try
            {
                dynamic key = ConvertInput(txtKey.Text, keyType);
                if (currentMap.ContainsKey(key))
                {
                    currentMap.Remove(key);
                    UpdateMapDisplay();
                    txtKey.Text = string.Empty;
                    txtValue.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Key not found in the map.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing entry: {ex.Message}");
            }
        }

        private void UpdateMapDisplay()
        {
            try
            {
                lstKeys.Items.Clear();
                lstValues.Items.Clear();
                lstMapItems.Items.Clear();
                txtcheckKey.Text = string.Empty;
                txtcheckvalue.Text = string.Empty;
                lblContainsKey.Text = string.Empty;
                lblContainsValue.Text = string.Empty;
                txtKey.Text = string.Empty;
                txtValue.Text = string.Empty;
                txtforall.Text = string.Empty;

                if (currentMap == null || currentMap.IsEmpty)
                {
                    lstMapItems.Items.Add("Map is empty.");
                    return;
                }

                foreach (var key in currentMap.Keys)
                {
                    lstKeys.Items.Add(key);
                }
                foreach (var value in currentMap.Values)
                {
                    lstValues.Items.Add(value);
                }
                foreach (var entry in currentMap)
                {
                    lstMapItems.Items.Add($"{entry.Key}: {entry.Value}");
                }
                lblCount.Text = $"Count: {currentMap?.Count ?? 0}";
                lblIsEmpty.Text = $"IsEmpty: {currentMap?.IsEmpty ?? true}";
            }
            catch (Exception ex)  // Use general exception for debugging
            {
                MessageBox.Show($"Error updating display: {ex.Message}");
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentMap == null)
            {
                MessageBox.Show("Please create a map first.");
                return;
            }

            MessageBox.Show("Immutable map created successfully!");

            // Disable Add and Remove buttons for UnmutableMap
            btnAdd.Enabled = false;
            btnRemove.Enabled = false;

            // Update the ListBox to reflect the new map
            UpdateMapDisplay();
        }

        private void btnCheckKey_Click(object sender, EventArgs e)
        {
            if (currentMap == null)
            {
                MessageBox.Show("Please create a map first.");
                return;
            }

            if (!ValidateInput(txtcheckKey.Text, keyType))
            {
                MessageBox.Show($"Invalid key format for {keyType.Name} type");
                return;
            }

            try
            {
                dynamic key = ConvertInput(txtcheckKey.Text, keyType);
                bool containsKey = currentMap.ContainsKey(key);
                lblContainsKey.Text = containsKey ? "Key exists." : "Key does not exist.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking key: {ex.Message}");
            }
        }
        private void btnCheckValue_Click(object sender, EventArgs e)
        {
            if (currentMap == null)
            {
                MessageBox.Show("Please create a map first.");
                return;
            }

            if (!ValidateInput(txtcheckvalue.Text, valueType))
            {
                MessageBox.Show($"Invalid value format for {valueType.Name} type");
                return;
            }

            try
            {
                dynamic valueToCheck = ConvertInput(txtcheckvalue.Text, valueType);
                bool containsValue = false;

                // Check each value in the map (need to handle type conversion)
                foreach (var entry in currentMap)
                {
                    dynamic mapValue = entry.Value;

                    if (valueType == typeof(string))
                    {
                        if (string.Equals(mapValue.ToString(), valueToCheck.ToString(), StringComparison.Ordinal))
                        {
                            containsValue = true;
                            break;
                        }
                    }
                    else if (mapValue == valueToCheck)
                    {
                        containsValue = true;
                        break;
                    }
                }

                lblContainsValue.Text = containsValue ? "Value exists." : "Value does not exist.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking value: {ex.Message}");
            }
        }

        private void btnfilter_Click(object sender, EventArgs e)
        {
            if (currentMap == null)
            {
                MessageBox.Show("Please create a map first.");
                return;
            }

            try
            {
                bool allValid = true;

                // Check all entries in the map
                foreach (var entry in currentMap)
                {
                    dynamic value = entry.Value;

                    // Check for empty based on type
                    if (value is string strValue)
                    {
                        if (string.IsNullOrEmpty(strValue))
                        {
                            allValid = false;
                            break;
                        }
                    }
                    else if (value == null)
                    {
                        allValid = false;
                        break;
                    }

                }

                MessageBox.Show(allValid ? "All entries are valid." : "Some entries are invalid.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking map entries: {ex.Message}");
            }
        }

        private void DisplayMap()
        {

            lstValues.Items.Clear();

            if (currentMap == null || currentMap.IsEmpty)
            {
                lstValues.Items.Add("Map is empty");
                return;
            }

            foreach (var entry in currentMap)
            {
                lstValues.Items.Add($"{entry.Value}");
            }
        }

        private void btnForAll_Click(object sender, EventArgs e)
        {
            string look = txtforall.Text;

            if (currentMap == null || currentMap.IsEmpty || string.IsNullOrEmpty(look))
            {
                MessageBox.Show("Please create and populate a map first");
                return;
            }

            try
            {
                lstKeys.Items.Clear();
                bool foundAny = false;

                foreach (var entry in currentMap)
                {
                    if (entry.Value.ToString().Contains(look, StringComparison.OrdinalIgnoreCase))
                    {
                        lstKeys.Items.Add($"{entry.Key} = {entry.Value}");
                        foundAny = true;
                    }
                }

                if (!foundAny)
                {
                    lstKeys.Items.Add("Not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in FindAll: {ex.Message}");
            }
        }

        private void btnforeach_Click(object sender, EventArgs e)
        {
            if (currentMap == null || currentMap.IsEmpty)
            {
                MessageBox.Show("Please create and populate a map first");
                return;
            }

            try
            {
                // Get keys in the most compatible way
                var keys = new List<dynamic>();
                foreach (var key in (IEnumerable)currentMap.Keys)
                {
                    keys.Add(key);
                }

                // Process each key-value pair
                foreach (var key in keys)
                {
                    try
                    {
                        dynamic value = currentMap[key];

                        switch (value)
                        {
                            case string str:
                                currentMap[key] = str.ToUpper();
                                break;
                            case int num:
                                currentMap[key] = num + 2;
                                break;
                            case double dbl:
                                currentMap[key] = dbl * 2;
                                break;
                            case bool b:
                                currentMap[key] = !b;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error processing key {key}: {ex.Message}");
                        continue;
                    }
                }

                UpdateMapDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ForEach operation: {ex.Message}\n\n{ex.GetType().Name}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (currentMap == null || currentMap.IsEmpty)
            {
                MessageBox.Show("Please create and populate a map first.");
                return;
            }

            try
            {
                // Clear previous chart data
                chart1.Series.Clear();
                chart1.Titles.Clear();

                // Create a new series
                var series = new Series("Map Values")
                {
                    ChartType = SeriesChartType.Doughnut, // Change to Line, Pie, etc. as needed
                    Color = Color.SteelBlue,
                    IsValueShownAsLabel = true
                };

                // Add data points from your map
                foreach (var entry in currentMap)
                {
                    double value = 0;

                    // Convert different types to double for charting
                    if (entry.Value is int i) value = i;
                    else if (entry.Value is double d) value = d;
                    else if (entry.Value is bool b) value = b ? 1 : 0;
                    else continue; // Skip non-numeric values

                    series.Points.AddXY(entry.Key.ToString(), value);
                }

                // Add series to chart
                chart1.Series.Add(series);

                // Customize chart appearance
                chart1.Titles.Add("Map Values Visualization");
                chart1.ChartAreas[0].AxisX.Title = "Keys";
                chart1.ChartAreas[0].AxisY.Title = "Values";
                chart1.ChartAreas[0].AxisX.Interval = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating graph: {ex.Message}");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (currentMap == null || currentMap.IsEmpty)
            {
                MessageBox.Show("Please create and populate a map first.");
                return;
            }

            try
            {
                // Clear previous chart data
                chart2.Series.Clear();
                chart2.Titles.Clear();

                // Create a new series for keys
                var series = new Series("Map Keys")
                {
                    ChartType = SeriesChartType.Doughnut, // Consistent with your values graph
                    Color = Color.Orange, // Different color for keys
                    IsValueShownAsLabel = true,
                    Font = new Font("Arial", 8, FontStyle.Bold)
                };

                // Add data points from your map's KEYS
                foreach (var entry in currentMap)
                {
                    double keyValue = 0;

                    // Convert key to plottable value (same logic as values)
                    if (entry.Key is int i) keyValue = i;
                    else if (entry.Key is double d) keyValue = d;
                    else if (entry.Key is bool b) keyValue = b ? 1 : 0;
                    else if (entry.Key is string s) keyValue = s.Length; // String length as numeric representation
                    else continue;

                    series.Points.AddXY(entry.Key.ToString(), keyValue);
                }

                // Add series to chart
                chart2.Series.Add(series);

                // Customize chart appearance
                chart2.Titles.Add("Map Keys Visualization");
                chart2.ChartAreas[0].AxisX.Title = "Keys";
                chart2.ChartAreas[0].AxisY.Title = "Key Metrics";
                chart2.ChartAreas[0].AxisX.Interval = 1;
                chart2.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Better readability
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating keys graph: {ex.Message}");
            }
        }
    }
}










/*  private void button3_Click(object sender, EventArgs e)
          {
              if (currentMap == null || currentMap.IsEmpty)
              {
                  MessageBox.Show("Map is empty or not created.");
                  return;
              }

              chart1.Series.Clear();
              chart1.ChartAreas.Clear();
              chart1.Legends.Clear();

              //Create a new chart area with dual Y-axes
              var chartArea = new ChartArea("MainArea");
              chartArea.AxisX.Interval = 1;
              chartArea.AxisX.LabelStyle.Angle = -45;
              chart1.ChartAreas.Add(chartArea);

             //  Series 1: Values (Primary Y-axis)
              var valueSeries = new Series("Values")
              {
                  ChartType = SeriesChartType.Column,
                  Color = Color.SteelBlue,
                  IsValueShownAsLabel = true,
                  YAxisType = AxisType.Primary
              };

              // Series 2: Keys (Secondary Y-axis)
              var keySeries = new Series("Keys")
              {
                  ChartType = SeriesChartType.Line,
                  Color = Color.Red,
                  IsValueShownAsLabel = true,
                  YAxisType = AxisType.Secondary
              };

             //  Process all entries
              int index = 0;
              foreach (var entry in currentMap)
              {
                  dynamic key = entry.Key;
                  dynamic value = entry.Value;

                  // Convert keys and values to plottable numbers
                  double keyPlotValue = ConvertToPlotValue(key);
                  double valuePlotValue = ConvertToPlotValue(value);

                   //Add data points
                  valueSeries.Points.AddXY(index, valuePlotValue);
                  keySeries.Points.AddXY(index, keyPlotValue);

                   //Custom X-axis labels
                  chartArea.AxisX.CustomLabels.Add(
                      new CustomLabel(index - 0.5, index + 0.5,
                                    $"{GetDisplayText(key)}|{GetDisplayText(value)}",
                                    0, LabelMarkStyle.None)
                  );

                  index++;
              }

              chart1.Series.Add(valueSeries);
              chart1.Series.Add(keySeries);
              chart1.Titles.Add("Key-Value Pair Visualization");
          }

           ///Helper: Convert any type to plottable double
          private double ConvertToPlotValue(dynamic input)
          {
              return input switch
              {
                  int i => i,
                  double d => d,
                  bool b => b ? 1 : 0,
                  string s => s.Length, // Use string length for visualization
                  _ => 0
              };
          }

          // Helper: Get display-friendly text
          private string GetDisplayText(dynamic input)
          {
              if (input == null) return "null";
              if (input is string s) return s.Length > 10 ? s.Substring(0, 7) + "..." : s;
              return input.ToString();
          }

         private void button4_Click(object sender, EventArgs e)
         {
             if (currentMap == null || currentMap.IsEmpty)
             {
                 MessageBox.Show("Map is empty or not created.");
                 return;
             }

             chart1.Series.Clear();
             var series = new Series("Map Data")
             {
                 ChartType = SeriesChartType.Doughnut, // Use Column/Bar/Pie for strings
                 IsValueShownAsLabel = true,
                 Color = Color.SteelBlue
             };

           //  Count string frequencies(for categorical data)
                 var stringCounts = new Dictionary<string, int>();

             foreach (var entry in currentMap)
             {
                 dynamic value = entry.Value;

                // Handle different types
                 if (value is string strValue)
                 {
                   //  Count string occurrences
                     if (!string.IsNullOrEmpty(strValue))
                     {
                         if (stringCounts.ContainsKey(strValue))
                             stringCounts[strValue]++;
                         else
                             stringCounts[strValue] = 1;
                     }
                 }
                 else if (value is int || value is double)
                 {
                  //   Directly plot numbers
                     series.Points.AddXY(entry.Key.ToString(), Convert.ToDouble(value));
                 }
             }

            // Add string data to chart(if any strings found)
             if (stringCounts.Count > 0)
             {
                 foreach (var item in stringCounts)
                 {
                     series.Points.AddXY(item.Key, item.Value);
                 }
             }

             chart1.Series.Add(series);
             chart1.Titles.Add("Map Visualization");
             chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Rotate labels for readability
         }*/