2D VERSION:

1) Create tiles
- Import/Create your prefab
- Add the script "Tile" to each one of your prefab

2) Create the input example
- Create an empty object
- Add the script "SimpleTiledModelRules" to your object
- Choose the size of your canvas 
- Add the tile you want to draw to the "Current Tile" variable and draw on the canvas

IMPORTANT: You can change the width and height whenever you want but make sure to clean after you changed it to delete tiles that have been removed from the canvas

You can clear to delete all tiles

3) Generate the rules
After filling in the entire canvas, click on "Generate Rules"

4) Output 
- Create an empty object
- Add the script "WFC" to your object
- Select the width and height
- Select your input example and add it to your "Rules" variable
- Click on "RUN" to run the entire function, "RUN one step" to run only one step (= one propagation)

IMPORTANT: Make sure you didn't forget to generate rules in your input either way it won't work

You can restart (= clear) the canvas by clicking on "RESTART"


