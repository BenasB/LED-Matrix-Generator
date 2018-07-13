# LED Matrix Generator
Generate graphics for LED matrices. To avoid misunderstandings, a combination of LED states (turned on/off) is called a graphic. My first try at creating programs with WPF.

[Download .exe](https://github.com/BenasB/LED-Matrix-Generator/raw/master/LEDMatrixGenerator.zip)

![Screenshot](https://i.imgur.com/LponlGO.png?1)

## Features
* Generates HEX/BIN codes
* Clear, enable all, invert and shift LEDs
* Resize matrix (Probably works only up to 32 leds per row)
* Collections

## How do collections work?
1. Create a collection (Collection -> New)
2. Draw your graphic
3. Name the graphic (Default: New Graphic)
4. Click Add to Collection
5. Repeat steps 2 - 4

* If you'd like to edit/delete a graphic from a collection, open (if one isn't already open) a collection (Collection -> Open) and edit it (Collection -> Edit). You will see a list of all the graphics in the collection.
* Collections can contain graphics of different sizes (e.g. 8x8 and 16x8)
* A collection can be exported to Arduino/C compatible code (Collection -> Export). This will create a .c file that you can include in your sketch. Note that whitespace from names will be removed.
* Collections are stored as .json files.
* It's not recommended to move the collection file (.json) while the collection is open in the program.

![Collection edit screenshot](https://i.imgur.com/mLLEflt.png?1)

### Dependencies
* Newtonsoft's Json.NET