# HauntedJaunt
 CS 410 Assignment 2

Haunted Jaunt new features:

1.) Used dot product to change area of detection from the point of view to a cone of detection for the gargoyles. The dot product is used to calculate the angle between the gargoyle and the player and if that angle is within half of the specified detection angle, the player will be "detected" by the gargoyle and has a set amount of time to leave the detection area before losing the game.

2.) Used linear interpolation to turn the gargoyle to face the player during detection. This is done in a similar manner to the in class example and uses cross product. A direction vector between the gargoyle and the player is calculated and then normalized and based on the cross product of that direction vector and the forward vector, the gargoyle angle is transformed.

3.) Added a particle effect to the gargoyle so there is a visual indicator of detection. I just added this and the sound effect into the conditional statement for detection and it was pretty straightforward.

4.) Added a sound effect to the gargoyle so an alarm sound plays during detection.
