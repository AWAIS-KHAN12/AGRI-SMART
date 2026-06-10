using System;
using System.Collections.Generic;
using System.Linq;
using AgriSmart.Web.Models;

namespace AgriSmart.Web.Services
{
    public class PestDiseaseService
    {
        private readonly List<PestDiseaseItem> _items = new List<PestDiseaseItem>
        {
            // Pests
            new PestDiseaseItem
            {
                Id = 1,
                Name = "Caterpillar",
                Type = "Pest",
                ShortDescription = "Hairy larvae that devour leaves.",
                Description = "Caterpillars are the larval stage of moths and butterflies. The hairy/spiny varieties found in crops are highly destructive defoliators. They consume tender leaves, stems, and buds, causing rapid and widespread damage to vegetable and fruit crops.",
                ImagePath = "images/caterpillar.png",
                Symptoms = "Irregular holes and ragged edges on leaves, complete defoliation of young plants, presence of hairy green or black-dotted larvae on leaf surfaces, dark frass (droppings) on leaves and under plants.",
                ControlMeasures = "Organic Control: Handpick and destroy larvae early in the morning. Spray Bacillus thuringiensis (Bt) which is highly effective against caterpillars. Introduce parasitic wasps as natural predators.\n\nChemical Control: Apply contact insecticides such as Chlorpyrifos, Lambda-cyhalothrin, or Spinosad. Ensure thorough coverage of the underside of leaves where larvae often hide."
            },
            new PestDiseaseItem
            {
                Id = 2,
                Name = "Wireworm",
                Type = "Pest",
                ShortDescription = "Root-feeding soil larvae.",
                Description = "Wireworms are the larval stage of click beetles (family Elateridae). They are hard-bodied, golden-orange cylindrical larvae that live in the soil for 3–5 years, feeding on plant roots, seeds, and underground stems, making them among the most damaging soil pests in agriculture.",
                ImagePath = "images/wireworm.png",
                Symptoms = "Seedling wilting and death, stunted plant growth, visible tunneling damage in tubers and roots (especially potatoes), poor germination, and thin patchy stands in fields.",
                ControlMeasures = "Organic Control: Deep-till soil before planting to expose larvae to birds and predators. Rotate crops to break the pest cycle. Avoid planting susceptible crops in heavily infested fields.\n\nChemical Control: Apply soil insecticides such as Chlorpyrifos, Tefluthrin, or Fipronil as a seed treatment or in-furrow application at planting time."
            },
            new PestDiseaseItem
            {
                Id = 3,
                Name = "Japanese Beetle",
                Type = "Pest",
                ShortDescription = "Beetles that skeletonize leaves.",
                Description = "Japanese beetles (Popillia japonica) are highly destructive invasive insects with metallic green and copper-brown bodies. Both adults and larvae cause damage — adults skeletonize leaves and feed on flowers and fruits, while larvae (grubs) feed on grass and plant roots underground.",
                ImagePath = "images/japanese_beetle.png",
                Symptoms = "Leaves with tissue eaten between veins leaving a lacy, skeletonized appearance. Damaged fruits and flowers. Brown irregular patches in lawns due to grub activity underground. Large groups of beetles aggregating on plants.",
                ControlMeasures = "Organic Control: Handpick beetles in the early morning when they are sluggish. Install pheromone beetle traps away from the garden. Apply Milky Spore (Bacillus popilliae) powder to soil to kill grubs.\n\nChemical Control: Apply systemic insecticides such as Imidacloprid or Carbaryl for adult control. For grubs, use soil-applied insecticides containing Chlorantraniliprole or Trichlorfon."
            },

            // Diseases
            new PestDiseaseItem
            {
                Id = 4,
                Name = "Powdery Mildew",
                Type = "Disease",
                ShortDescription = "White powdery fungal growth.",
                Description = "Powdery mildew is a common fungal disease that affects leaves, stems, and flowers. It is favored by warm, dry days and cool, damp nights, creating a powdery coating that blocks sunlight and hampers photosynthesis.",
                ImagePath = "images/powdery_mildew.png",
                Symptoms = "White to grayish powdery spots or patches on the upper surface of leaves, leaf curling, distorted shoot growth, and premature yellowing and drying of leaves.",
                ControlMeasures = "Organic Control: Spray with a baking soda and liquid soap solution, apply sulfur-based organic fungicides, ensure adequate spacing for airflow, and avoid overhead watering.\n\nChemical Control: Apply systemic fungicides such as Myclobutanil, Penconazole, or Triadimefon upon first appearance of symptoms."
            },
            new PestDiseaseItem
            {
                Id = 5,
                Name = "Leaf Spot",
                Type = "Disease",
                ShortDescription = "Brown spots on leaves.",
                Description = "Leaf spot is caused by various fungal or bacterial pathogens. It typically starts on lower leaves that receive less airflow and more moisture, creating necrotic spots that can lead to complete defoliation.",
                ImagePath = "images/leaf_spot.png",
                Symptoms = "Circular, dark brown, black, or tan spots on leaves, often surrounded by a yellow halo. Spots may coalesce to form larger dead patches, leading to leaf drop.",
                ControlMeasures = "Organic Control: Prune and destroy infected leaves, avoid wetting foliage during irrigation, apply copper-based organic fungicides, and clean tools to prevent cross-contamination.\n\nChemical Control: Apply broad-spectrum protective fungicides containing Chlorothalonil, Mancozeb, or Carbendazim."
            },
            new PestDiseaseItem
            {
                Id = 6,
                Name = "Blast",
                Type = "Disease",
                ShortDescription = "Affects leaves and grains.",
                Description = "Blast (commonly Rice/Wheat Blast) is a highly destructive fungal disease caused by Pyricularia oryzae. It can attack plants at all growth stages, leading to severe yield loss in major cereal crops.",
                ImagePath = "images/blast.png",
                Symptoms = "Spindle-shaped or diamond-shaped lesions on leaves with brown or reddish borders and gray centers. Affected stems may choke and break (neck blast), leading to empty, white panicles.",
                ControlMeasures = "Organic Control: Cultivate resistant crop varieties, maintain optimal nitrogen fertilizer levels, destroy crop residues after harvest, and keep fields properly flooded.\n\nChemical Control: Use seed treatment fungicides and spray Tricyclazole, Azoxystrobin, or Isoprothiolane at the heading or booting stages."
            }
        };

        public List<PestDiseaseItem> GetAllItems()
        {
            return _items;
        }

        public List<PestDiseaseItem> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return _items;

            query = query.Trim().ToLower();
            return _items.Where(x => 
                x.Name.ToLower().Contains(query) || 
                x.ShortDescription.ToLower().Contains(query) || 
                x.Description.ToLower().Contains(query) ||
                x.Symptoms.ToLower().Contains(query)
            ).ToList();
        }

        public PestDiseaseItem GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }
    }
}
