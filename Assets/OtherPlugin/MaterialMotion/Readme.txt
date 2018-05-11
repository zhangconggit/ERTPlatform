---------------------------------------------------------------------------------------------------------

Thank you for your interesst in MaterialMotion!

This component is for handling shader property animation on a material level. 
It features preview ability in edit mode and easy layering of motions.

---------------------------------------------------------------------------------------------------------

Useage:

 	-drag&drop or add component "Mesh/MaterialMotion" on a gameobject with a renderer.
 	-select the material you want to animate
 	-select the shader property (has to be a texture, color or float) to animate
 	-setup the curves/color to your liking - keep attention to the curve wrapmodes!
 	-assign a speed 	
 	
	show animation in editor -> should the script update in edit mode all the time or only in playmode 	
	
	for texture properties there are several options to choose from: 	
		-scroll: moving in u or v
		-scale: scale in u or v
		-flipbook: cycle over a texture sheet by scaling&offsetting uvs 
 	
---------------------------------------------------------------------------------------------------------

Remarks:
		
	you can layer motions freely - but be aware of sideeffects when updating the same properties.
	Use the ShiftUV helper component to offset UV's at startup to randomize offset - see "wind" in example.
	
---------------------------------------------------------------------------------------------------------

Problems and Errors or Questions:

	Although I haven taken much care, this software might still have some bugs, feel free to bugreport!
	I will try to resolve as quickly as possible.

	If you've got other questions don't hesitate to contact me.
	
	Contact: support@game-bakery.com

---------------------------------------------------------------------------------------------------------

Have fun creating stuff. 

---------------------------------------------------------------------------------------------------------