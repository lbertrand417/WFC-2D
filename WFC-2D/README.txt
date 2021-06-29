2D VERSION:

/!\ Data backup is not handled so when you reopen a scene it's better to restart from scratch

1) Create tiles
- Import/Create your prefab. Your prefab must be of one unit size and the pivot point at the middle
- Add the script "Tile" to each one of your prefab

2) Create the input example
- Create an empty object
- Add the script "SimpleTiledModelRules" to your object
- Choose the size of your canvas 
- Add the tile you want to draw to the "Current Tile" variable and draw on the canvas

IMPORTANT: You can change the width and height whenever you want but make sure to clean after you changed it to delete tiles that have been removed from the canvas

You can clear to delete all tiles.

3) Generate the rules
After filling in the ENTIRE canvas, click on "Generate Rules"

4) Output 
- Create an empty object
- Add the script "WFC" to your object
- Select the width and height
- Select your input example and add it to your "Rules" variable
- Click on "RUN" to run the entire function, "RUN one step" to run only one step (= one propagation)

IMPORTANT: Make sure you didn't forget to generate rules in your input either way it won't work

You can restart the algorithm by clicking on "RESTART". 

IMPORTANT: We encourage you to use RESTART whenever you modify one of the variable (either the size or the input).


