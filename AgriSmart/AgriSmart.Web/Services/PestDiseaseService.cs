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
                Name = "Aphids",
                Type = "Pest",
                ShortDescription = "Small green insects on leaves.",
                Description = "Aphids are small sap-sucking insects that colonize the tender stems and leaves of plants. They multiply rapidly and secrete a sticky substance called honeydew, which attracts ants and promotes sooty mold growth.",
                ImagePath = "images/aphids.png",
                Symptoms = "Curled or distorted leaves, yellowing foliage, stunted plant growth, sticky honeydew residue on leaves, and presence of tiny green, black, or peach-colored insects in clusters.",
                ControlMeasures = "Organic Control: Spray plants with a strong stream of water to dislodge them, apply organic neem oil or insecticidal soap, and introduce natural predators like ladybugs.\n\nChemical Control: If infestation is severe, apply systemic insecticides containing Imidacloprid or Thiamethoxam according to manufacturer guidelines."
            },
            new PestDiseaseItem
            {
                Id = 2,
                Name = "Whitefly",
                Type = "Pest",
                ShortDescription = "Sucks sap from leaves.",
                Description = "Whiteflies are tiny, sap-sucking winged insects related to aphids. They are typically found on the undersides of leaves, weakening the plant by draining vital nutrients and transmitting destructive plant viruses.",
                ImagePath = "images/whitefly.png",
                Symptoms = "Yellowing of leaves, premature leaf drop, sticky honeydew coating, and a visible cloud of tiny white moth-like insects that flutter when the plant is shaken or disturbed.",
                ControlMeasures = "Organic Control: Install yellow sticky traps to capture adult flies, spray the undersides of leaves with neem oil, or introduce predatory lacewings.\n\nChemical Control: Apply foliar sprays containing Acetamiprid, Bifenthrin, or Cypermethrin in rotation to prevent resistance."
            },
            new PestDiseaseItem
            {
                Id = 3,
                Name = "Fruit Borer",
                Type = "Pest",
                ShortDescription = "Larvae bore into fruits.",
                Description = "Fruit borers are the larval stages of moths that bore into developing vegetable and fruit crops. They feed internally, destroying the crop from the inside and making it completely unmarketable.",
                ImagePath = "images/fruit_borer.png",
                Symptoms = "Visible entry holes on fruits (like tomatoes, eggplants, or peppers), dark insect droppings (frass) near the holes, premature fruit ripening and dropping, and internal decay.",
                ControlMeasures = "Organic Control: Handpick and destroy infested fruits and larvae immediately, install pheromone traps to capture male moths, and spray Bacillus thuringiensis (Bt) or Spinosad.\n\nChemical Control: Apply targeted larvicides like Chlorantraniliprole or Flubendiamide during early larval stages before they enter the fruits."
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
