using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArrangementFilterPage : ContentPage
    {
        List<InventoryTypeDTO> inventoryTypeList = new List<InventoryTypeDTO>();

        List<PlantInventoryDTO> plants = new List<PlantInventoryDTO>();
        List<PlantTypeDTO> plantTypes = new List<PlantTypeDTO>();
        List<PlantNameDTO> plantNames = new List<PlantNameDTO>();

        List<FoliageInventoryDTO> foliage = new List<FoliageInventoryDTO>();
        List<FoliageTypeDTO> foliageTypes = new List<FoliageTypeDTO>();
        List<FoliageNameDTO> foliageNames = new List<FoliageNameDTO>();

        List<MaterialInventoryDTO> materials = new List<MaterialInventoryDTO>();
        List<MaterialTypeDTO> materialTypes = new List<MaterialTypeDTO>();
        List<MaterialNameDTO> materialNames = new List<MaterialNameDTO>();

        List<ContainerTypeDTO> containerTypes = new List<ContainerTypeDTO>();
        List<ContainerNameDTO> containerNames = new List<ContainerNameDTO>();
        List<ContainerInventoryDTO> containers = new List<ContainerInventoryDTO>();

        public ArrangementFilterPage()
        {
            InitializeComponent();

            inventoryTypeList = ((App)App.Current).GetInventoryTypes();

            ObservableCollection<KeyValuePair<long, string>> list1 = new ObservableCollection<KeyValuePair<long, string>>();
            foreach (InventoryTypeDTO type in inventoryTypeList)
            {
                list1.Add(new KeyValuePair<long, string>(type.InventoryTypeId, type.InventoryTypeName));
            }

            InventoryType.ItemsSource = list1;

            InventoryType.SelectedIndexChanged += InventoryType_SelectedIndexChanged;

            Type.SelectedIndexChanged += TypeCombo_SelectionChanged;

            ArrangementInventoryList.ItemSelected += ArrangementInventoryList_ItemSelected;
        }

        private void ArrangementInventoryList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ArrangementInventoryFilteredItem item = (sender as ListView).SelectedItem as ArrangementInventoryFilteredItem;

            WorkOrderInventoryItemDTO wo = new WorkOrderInventoryItemDTO();

            if (item != null)
            {

                wo = new WorkOrderInventoryItemDTO(0,item.Id,item.Name,0);

                //save this to a variable in app
                int debug = 1;
                //    if (arrangementParentWnd != null)
                //    {
                //        arrangementParentWnd.AddInventorySelection(item.Id, item.Name);
                //        arrangementParentWnd = null;
                //    }
                //    else if (workOrderParentWnd != null)
                //    {
                //        workOrderParentWnd.AddInventorySelection(item.Id, item.Name);
                //        workOrderParentWnd = null;
                //    }
                //    else if (shipmentParentWnd != null)
                //    {
                //        shipmentParentWnd.AddInventorySelection(item.Id, item.Name);
                //        shipmentParentWnd = null;
                //    }
            }
            //this.Close();

            MessagingCenter.Send<ArrangementFilterPage, WorkOrderInventoryItemDTO>(this, "UseFilter", wo);

            Navigation.PopModalAsync();
        }

        private void InventoryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            long selectedValue = ((KeyValuePair<long, string>)InventoryType.SelectedItem).Key;

            //get types for the current inventory type selection
            Picker p = sender as Picker;
            KeyValuePair<long, string> kvp = (KeyValuePair<long, string>)p.SelectedItem;

            ObservableCollection<KeyValuePair<long, string>> list1 = new ObservableCollection<KeyValuePair<long, string>>();

            switch (kvp.Value)
            {
                case "Orchids":
                    if (plantTypes.Count == 0)
                    {
                        plantTypes = ((App)App.Current).GetPlantTypes();
                    }

                    foreach (PlantTypeDTO plantType in plantTypes)
                    {
                        list1.Add(new KeyValuePair<long, string>(plantType.PlantTypeId, plantType.PlantTypeName));
                    }

                    break;

                case "Foliage":
                    if (foliageTypes.Count == 0)
                    {
                        foliageTypes = ((App)App.Current).GetFoliageTypes();
                    }

                    foreach (FoliageTypeDTO foliage in foliageTypes)
                    {
                        list1.Add(new KeyValuePair<long, string>(foliage.FoliageTypeId, foliage.FoliageTypeName));
                    }
                    break;

                case "Materials":
                    if (materialTypes.Count == 0)
                    {
                        materialTypes = ((App)App.Current).GetMaterialTypes();
                    }

                    foreach (MaterialTypeDTO materialType in materialTypes)
                    {
                        list1.Add(new KeyValuePair<long, string>(materialType.MaterialTypeId, materialType.MaterialTypeName));
                    }
                    break;

                case "Containers":
                    if (containerTypes.Count == 0)
                    {
                        containerTypes = ((App)App.Current).GetContainerTypes();
                    }

                    foreach (ContainerTypeDTO container in containerTypes)
                    {
                        list1.Add(new KeyValuePair<long, string>(container.ContainerTypeId, container.ContainerTypeName));
                    }
                    break;
            }

            Type.ItemsSource = list1;

            List<string> sizes = ((App)App.Current).GetSizeByInventoryType(kvp.Key);

            ObservableCollection<string> list2 = new ObservableCollection<string>();

            foreach(string s in sizes)
            {
                list2.Add(s);
            }

            Size.ItemsSource = list2;
        }

        private void TypeCombo_SelectionChanged(object sender, EventArgs e)
        {
            string inventoryType = ((KeyValuePair<long, string>)InventoryType.SelectedItem).Value;

            //get names for the current inventory type selection
            Picker cb = sender as Picker;
            KeyValuePair<long, string> kvp = (KeyValuePair<long, string>)cb.SelectedItem;

            ObservableCollection<KeyValuePair<long, string>> list1 = new ObservableCollection<KeyValuePair<long, string>>();

            switch (inventoryType)
            {
                case "Orchids":
                    plantNames = ((App)App.Current).GetPlantNamesByType(kvp.Key);

                    foreach (PlantNameDTO plantName in plantNames)
                    {
                        list1.Add(new KeyValuePair<long, string>(plantName.PlantNameId, plantName.PlantName));
                    }

                    Name.ItemsSource = list1;

                    break;

                case "Foliage":
                    foliageNames = ((App)App.Current).GetFoliageNamesByType(kvp.Key);

                    foreach (FoliageNameDTO foliageName in foliageNames)
                    {
                        list1.Add(new KeyValuePair<long, string>(foliageName.FoliageNameId, foliageName.FoliageName));
                    }

                    Name.ItemsSource = list1;

                    break;

                case "Materials":
                    materialNames = ((App)App.Current).GetMaterialNamesByType(kvp.Key);

                    foreach (MaterialNameDTO materialName in materialNames)
                    {
                        list1.Add(new KeyValuePair<long, string>(materialName.MaterialNameId, materialName.MaterialName));
                    }

                    Name.ItemsSource = list1;

                    break;

                case "Containers":
                    containerNames = ((App)App.Current).GetContainerNamesByType(kvp.Key);

                    foreach (ContainerNameDTO containerName in containerNames)
                    {
                        list1.Add(new KeyValuePair<long, string>(containerName.ContainerNameId, containerName.ContainerName));
                    }

                    Name.ItemsSource = list1;
                    break;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            string inventoryType = ((KeyValuePair<long, string>)InventoryType.SelectedItem).Value;

            long typeId = ((KeyValuePair<long, string>)Type.SelectedItem).Key;

            string name = String.Empty;
            if (Name.SelectedItem != null)
            {
                name = ((KeyValuePair<long, string>)Name.SelectedItem).Value;
            }
            ObservableCollection<ArrangementInventoryFilteredItem> list1 = new ObservableCollection<ArrangementInventoryFilteredItem>();

            switch (inventoryType)
            {
                case "Orchids":
                    plants = ((App)App.Current).GetPlantsByType(typeId).PlantInventoryList;

                    if (!String.IsNullOrEmpty(name))
                    {
                        plants = plants.Where(a => a.Plant.PlantName.Contains(name)).ToList();
                    }

                    foreach (PlantInventoryDTO p in plants)
                    {
                        list1.Add(new ArrangementInventoryFilteredItem()
                        {
                            Id = p.Inventory.InventoryId,
                            Type = p.Inventory.InventoryName,
                            Name = p.Plant.PlantName,
                            Size = p.Plant.PlantSize,
                            ServiceCode = p.Inventory.ServiceCodeName
                        });
                    }
                    break;

                case "Foliage":
                    foliage = ((App)App.Current).GetFoliageByType(typeId).FoliageInventoryList;

                    if (!String.IsNullOrEmpty(name))
                    {
                        foliage = foliage.Where(a => a.Foliage.FoliageName.Contains(name)).ToList();
                    }

                    foreach (FoliageInventoryDTO f in foliage)
                    {
                        list1.Add(new ArrangementInventoryFilteredItem()
                        {
                            Id = f.Inventory.InventoryId,
                            Type = f.Inventory.InventoryName,
                            Name = f.Foliage.FoliageName,
                            Size = f.Foliage.FoliageSize,
                            ServiceCode = f.Inventory.ServiceCodeName
                        });
                    }
                    break;

                case "Materials":
                    materials = ((App)App.Current).GetMaterialByType(typeId).MaterialInventoryList;

                    if (!String.IsNullOrEmpty(name))
                    {
                        materials = materials.Where(a => a.Material.MaterialName.Contains(name)).ToList();
                    }

                    foreach (MaterialInventoryDTO m in materials)
                    {
                        list1.Add(new ArrangementInventoryFilteredItem()
                        {
                            Id = m.Inventory.InventoryId,
                            Type = m.Inventory.InventoryName,
                            Name = m.Material.MaterialName,
                            Size = m.Material.MaterialSize,
                            ServiceCode = m.Inventory.ServiceCodeName
                        });
                    }
                    break;

                case "Containers":
                    containers = ((App)App.Current).GetContainersByType(typeId).ContainerInventoryList;

                    if (!String.IsNullOrEmpty(name))
                    {
                        containers = containers.Where(a => a.Container.ContainerName.Contains(name)).ToList();
                    }

                    foreach (ContainerInventoryDTO c in containers)
                    {
                        list1.Add(new ArrangementInventoryFilteredItem()
                        {
                            Id = c.Inventory.InventoryId,
                            Type = c.Container.ContainerTypeName,
                            Name = c.Inventory.InventoryName,
                            Size = c.Container.ContainerSize,
                            ServiceCode = c.Inventory.ServiceCodeName
                        });
                    }
                    break;
            }

            ArrangementInventoryList.ItemsSource = list1;
        }
    }
}