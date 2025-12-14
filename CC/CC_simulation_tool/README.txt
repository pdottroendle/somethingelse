ABCDOS UI Rendering & SEED Wiring — Specification (vFinal)

Document purpose: This specification defines the rendering, neighbor semantics, SEED evaluation, and between‑box wiring rules for the ABCDOS grid UI. Each section has a reference number (e.g., Spec‑3) that matches a comment block in the final code file so reviewers can trace implementation to requirements.


Spec‑1 — Global Config
Code block: [Spec-1] Global Config
Purpose: Central, fixed configuration (no parameterization) for visuals and behavior.

LANES: [Index 0,1,2,3] 4 (on each ports per side or corner cluster).
PORTS: [Alias A,B,C,D,O,S ax,bx,cx,dx,ox,sx] 12 styling: radius 2.5px, stroke color #333.
DIRECTION:[Group forward, backward] 2 
  forward Alias A,B,C,D,O,S normal line, 
  backwards Alias ax,bx,cx,dx,ox,sx] dotted line [see Spec 15]    
BOXES: [Order counted left top towards right, then next line untill the bottom] NxN

Wire styling base: color #32CD32, width 2px, round caps.
Geometry offsets: corner cluster offset 6px, side margin 6px.

Spec‑2 — Grid Geometry
Code block: [Spec-2] Grid Geometry
Purpose: Defines grid dimensions and helpers.

grid = { cols, rows, boxW, boxH, gapX, gapY }
isEdgeBox(x,y): true if the box lies on the outer boundary.
rectForBox(bx,by): returns {x, y, w, h} for the box’s canvas rect.


Spec‑3 — Port Anchors
Code block: [Spec-3] Port Anchors
Purpose: Fixed physical locations of ports per your diagram—no parameterization.

Forward (UPPERCASE Alias):

A → TOP (4 horizontally spaced dots)
B → LEFT (4 vertically spaced dots)
O → TOP‑LEFT corner cluster (4 spaced dots at 90 degrees of the diagonal OS)
C → RIGHT (4 vertically spaced dots)
D → BOTTOM (4 horizontally spaced dots)
S → BOTTOM‑RIGHT corner cluster (4 spaced dots at 90 degrees of the diagonal OS)


Backward (same physical positions, different labels, LOWERCASE Alias):
ax, bx, ox, cx, dx, sx


Spec‑4 — Port Rendering
Code block: [Spec-4] Port Rendering
Purpose: Ports (small circles) are always drawn; no fill color to indicate bit values.

Spec‑5 — SEED Parsing
Code block: [Spec-5] SEED Parsing
Purpose: Extract output alias/lane and optional expression.

Parses strings like:

"A[0]", "A[1] # !B[0]", "ax[0] && N", "O[3] || N".


Detects:

alias (uppercased), idx (lane), expr (optional)
edgeGuarded (if the string starts with backslash)
usesN (true if expr contains N as a standalone token)


Spec‑6 — Box Attribute N
Code block: [Spec-6] Box Attribute N
Purpose: Simplify NULL handling by using N as a box attribute:

N has no index and acts like a boolean variable in SEED expressions.

Example: A[0] && N draws a wire only on edge boxes for which A[0] evaluates to 1.


Spec‑7 — Neighbor Mapping & Counterparts
Code block: [Spec-7] Neighbor Mapping + Counterparts
Purpose: Resolve the neighbor location for an output and the counterpart input port.

Forward:

A → neighbor above (x, y−1) → counterpart D (bottom)
B → neighbor left (x−1, y) → counterpart C (right)
O → neighbor up‑left (x−1, y−1) → counterpart S (down-right corner)

Backward:

ax → below → dx
bx → right → cx
ox → down‑right →sx


Spec‑8 — Zoom Module
Code block: [Spec-8] Zoom Module
Purpose: Provide interactive zoom via mouse wheel and slider (50%–400%).

Zoom applies a canvas transform; redraw is requested after each change.


Spec‑9 — Wire Style (Adaptive Blur)
Code block: [Spec-9] Wire Style
Purpose: Make wires readable at any scale.

Adaptive blur grows with local density and shrinks with zoom‑in.
styleForLines(scale, lineCount) returns dynamic width, blur, and alpha.
drawWire(...) sets shadowBlur, globalAlpha, and uses round caps.


Spec‑10 — Output‑Only Wire Decision
Code block: [Spec-10] Output-only wire decision
Purpose: Wires represent SEED augmentation only.

Draw a wire only if:
Output alias (A,B,O or ax,bx,ox)
SEED evaluates to 1 (via evaluator hook)
If the expression uses N, wire is shown only on EDGE boxes.
Pass‑through (A→D, B→C, O→S, dx→ax, cx→bx, sx→ox) does not draw any wire.

Spec‑11 — Port Anchor Lookup

Find the correct point for a given alias & lane in the current order (forward/backward) to draw wires between boxes.

Spec‑12 — Box Renderer

For each box draw frame in grey color with in the center the [Order #] Box ID
and always draw for each [Alias] ports 4 small circles showing the [Index] ID

They are placed in the central part of the edge spread on the 1/3 of box edge length
Alias A    B      C      D        O         S        forward group 
Alias ax   bx     cx     dx       ox        sx       backwards group
Edge  TOP  LEFT   RIGHT  BOTTOM   TOP LEFT  BOTTOM RIGHT

Spec‑13 — NULL for N gating (SEED)

returns N = 1 for Boxes in the Last Column to the Right.

Spec‑14 — Redraw + Signal colors out of the Boxes

For each SEED: Parse ([Spec‑5]), check output alias, resolve neighbor ([Spec‑7]), evaluate, then decide ([Spec‑10]) and draw wire ([Spec‑9]).

Signals withg values =  0 tracks not shown, transparent
Signals withg values =  1 tracks shown, dark blue

Spec‑15 — Return Path distinct drawing 

The forward [Group] connections are shown in normal lines the backwards in dotted lines

Spec‑16 — Redraw + SEEDS colors inside the Boxes only

SEED Showing only N gating initiated Signals with Values = 1, in light blue color 
SEED Showing only all other initiated Signals with Values = 1, in Green color 
SEED PASS THROUGH default not shown, transparent

example SEED with N gating
Forward: ['O[0] && N', , 1]
Backward: ['cx[0] && N', 0, ]


================================================================================
Test Data

Hi Tester, here some pointers


		// ABCDOS
		// 3 directional (atomic element) 
		// 2x3x4 inputs : bi-directional x 3 directions (x,y,z axis) x  4 channels 
		//                    [Group]           [Alias]                   [Index]
                // Box Ordinal # [Order] tells where it is in the 2D array                  
                //      O -------A--------
		//      |ox     ax      |      Forwards:      Backwards:
		//      |  \    |       |      -----x-axis      
		//      |               |      |\                     |
		//     B|bx--  SEED --cx|C     | \                 \  |
		//      |               |      |  \                 \ |    
		//      |        |    \ |      |   \ z-axis          \|
		//      |       dx    sx|      y-axis           ------
		//      ----------------
		//             D         S 

1 You enter the Booleans Global SEED defining each output (using any inputs you want and any logic you decide)
2 You enter the top row y INPUTA and left side colum x INPUTB they are the global variables arriving to it
3 Optionally hard code locally one and only input ADHOC in any box [# Order] (simulates a space radiation damage)

SEED INPUTA INPUTB and ADHOC are fields in the form that allow the user to enter the relevant values
1 SEED is the Systolic array defined Boolean
2 INPUT A &B are the Test Vectors
3 ADHOC is the singularity or simulated glitch for Test purpose 

Example INPUT A = 19 hex = 0001, 1001 note : this is [3:0] Endianness used by Hardware, FPGA, ASIC

Box #0                               Box #1
"forward": [                         "forward": [ 
  { "D": [1 ,A1,A2,A3] },              { "D": [1 ,A1,A2,1 ] },  
  { "C": [B0,B1,B2,B3] },              { "C": [B0,B1,B2,B3] }, 
  { "S": [O0,O1,O2,O3] }               { "S": [O0,O1,O2,O3] }
]                                    ] 
note: its reverse Endian [0:3] used by this simulation Javascript, and normal number order from left to right
MSB : 3rd bit is the most significant bit 
LSB : oth bit is the least significant bit

Added for the EDGE SEED the symbol \ NULL so it does the Boolean only when in the edge Box
i.e cx[0] = \A[0] 

Added for simplicity, any Boolean that passes through what it receives does not need to be explicitely mentionned
"D": [ ,0 ,1 , 1 ]  means that it takes by default A[0] instead of the empty space "D": [A[0] ,0 ,1 , 1 ]  
 "C": [,,,,] or  "C": [] or omitting C means the default s PASS THROUGH "C": B0,B1,B2,B3]

ADHOC used to detect Loops when the user is feeding backwards signals
The ADHOC update field allows injection of one single point but that point can be any input of any box (anywhere) - ideally a random generator should run and automatically check if the outputs are always stable one clock later when the glitch is removed (this allows to detect Loops automatically) . Normally the outputs will always revert to the initial value one clock later after the disruptive signal is removed
the ADHOC simulation shows the output in such case but one clock later the value must revert tot  he initial value if not its a LATCHUP (Loop Alert). Its also useful to check what happens is electromagnetic or Photonic/Alpha (cosmic) radiation affect the Array in Flight situations.  




/*=================================================================================
each box has its own boolObject and boxes values
check that evaluateBoolObject is evaluating the boxes values, 
together with booleans from the masterBoolObject, 
and putting the result inside the respective boolObject

1. the user enters in 2 inputs box strings of hex called A and B 
2. the input event causes the following:
3. create an array of boxes (each withe their own json object and bool object): lenght of A x length of B, in term of number of hex characters  
4. each hex  is converted in a nibble[3..0] and populates a json object instance in its relevant place 
json object:
{
  "forward": [{
      "A": [0,0,0,0]
    },{
      "B": [0,0,0,0]
    },{
      "O": [0,0,0,0]
    }],
"backward": [{
      "dx": [0,0,0,0]
    },{
      "cx": [0,0,0,0]
    },{
      "sx": [0,0,0,0]
    }]
}
4. the json object instances updated by A populate the y axis left edge of the boxes array
5. the json object instances updated by B instances populate the x axis top edge of the boxes array
6. the rest of the boxes array are json objects unchanged
7. there is a MasterboolObject that the user can update (input field showing its value so the user can update it) 
MasterboolObject = {
            forward: [
                { D: ['A[0]', 0, 0, 0] },
                { C: ['B[0]', 0, 0, 0] },
                { S: [ 'A[0]&&B[0] || O[0]', 0, 0, 0] }
            ],
            backward: [
                { ax: [0, 0, 0, 0] },
                { bx: [0, 0, 0, 0] },
                { ox: [0, 0, 0, 0] }
            ]
        };
 8. each box will use its own json object and apply the MasterboolObject using it to create the 
boolObject 
boolObject = {
            forward: [
                { D: [0, 0, 0, 0] },
                { C: [0, 0, 0, 0] },
                { S: [0, 0, 0, 0] }
            ],
            backward: [
                { ax: [0, 0, 0, 0] },
                { bx: [0, 0, 0, 0] },
                { ox: [0, 0, 0, 0] }
            ]
        };
9. a canvas 800x800 animation should start showing all the boxes (1/3 sqare box and 2/3 of the inbetween space)
10. D should yield a y axis line connecting middle to the next bottom box setting its A value identical to it (except the top edge boxes)
11. C should yield a x axis line connecting middle to the next right box setting its B value identical to it (except the left edge boxes)
12. S should yield a z axis line connecting middle to the next bottom right box setting its O value identical to it (except the top and left edge boxes)
13. draw very fine and parallel lines for each corresponding bool Object that is = 1
14. start from the top left box go to the right and do it one line at the time from topo to bottom in 1 second time approximately		
// DEBUG
// console.log(i +' : boxes.length : '+ boxes.length + 'xscale' + xscale + '(i % xscale === 0)' + (i % xscale === 0));
// console.log( '(i < xscale || i % xscale === 0)' + (i < xscale || i % xscale === 0)); 
// Parse the JSON string back to an object
// const boolObject = masterBoolObject; // wont work the destination backpropagates back to the origin, because its the way JS handles the memory 
// console.log(i +' bool ========================== Evaluation of Boolean SEED',  formatJSON(boolObject.forward));
// We need to create a uphill barrier, stringify then parse.
=========================================================================
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Dynamic Input Processing and Evaluation System</title>
	<style>
        .container { display: flex; }
        .box {width: 33%;padding: 10px;}
		#topRightImage {margin-left:10px;z-index: 1000;width: 450px;height: auto;}
		#bodytext {margin-left:250px;margin-top:-160px;}
		#bodytext2 {margin-left:440px;margin-top:-250px;}
		#userInput {font-size:12px;font:Arial};
    </style>
	</head><body>
	Apr 13 3:50 AM PST - WA, USA &nbsp;&nbsp;
	<label for="inputA"></label>
    <input type="text" id="inputA" placeholder="Enter hex string A">
    <label for="inputB"></label>
    <input type="text" id="inputB" placeholder="Enter hex string B">
	<input type="text" id="updateInput" placeholder="Test set 0/B[2] 0.0.1.0=1">
	<h1> Dynamic Input Processing and Evaluation System</h1>	
	<img src="Scale.jpg" id="topRightImage" alt="scale map">
	<div id="bodytext">
    <script>//<input type="text" id="userInput" value='{"forward":[{"A":["A[0]",0,0,0]},{"B":["B[0]",0,0,"A[1] # B[1]"]},{"O":["A[0] && B[1]",0,"A[2] && !O[0]",0]}],"backward":[{"dx":[0,0,0,0]},{"cx":[0,0,0,0]},{"sx":[0,0,0,0]}]}' /></script>
    <label for="userInput"></label>
    <textarea id="userInput" rows="10" cols="21" oninput="updateMasterBool()"></textarea></div>
	<div id="bodytext2">
	<label for="masterBoolDisplay"></label>
    <pre id="masterBoolDisplay"></pre>
    <br><br></div><br><br><br><br><br><br>
    <canvas id="canvas" width="800" height="800"></canvas>
    <script>
        window.addEventListener('resize', () => {
            const img = document.getElementById('topRightImage');
            const bodytext = document.getElementById('bodytext');
            const bodytext2 = document.getElementById('bodytext2');
            const canvas = document.getElementById('canvas');
            
            img.style.width = window.innerWidth / 2 + 'px';
            
            // Adjust the text box positions based on the image size
            const imgRect = img.getBoundingClientRect();
            bodytext.style.left = imgRect.left + imgRect.width * 0.3 + 'px';
            bodytext.style.top = imgRect.top + imgRect.height * 0.4 + 'px';
            bodytext2.style.left = imgRect.left + imgRect.width * 0.6 + 'px';
            bodytext2.style.top = imgRect.top + imgRect.height * 0.4 + 'px';
            
            // Adjust the canvas position
            canvas.style.marginTop = '20px';
            canvas.style.marginLeft = 'auto';
            canvas.style.marginRight = 'auto';
        });

        // Initial adjustment
        window.dispatchEvent(new Event('resize'));

