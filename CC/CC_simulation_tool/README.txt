[Disclaimer:
These notes are preserved exactly as originally written on the date of creation. They represent raw, historical thoughts captured during the design process and are maintained without edits to protect the integrity of the original reasoning.
Thank you for understanding the depth of this work: communication feels secondary when the mind is fully engaged in ultra-low-level logic design. Spending decades at the transistor/LUT/flip-flop level is a completely different cognitive load compared to higher-level languages, so expressing thoughts in natural language can be harder when optimizing Boolean logic for FPGA.
Software engineers often benefit from abstractions, compilers, and frameworks, while FPGA engineers work at the raw hardware description level, where every gate matters. That explains why mental energy is focused on precision rather than conversational nuance.
These notes do not reflect current views or opinions beyond their historical context. If any reader finds certain lines irrelevant or uncomfortable, please kindly disregard them. The purpose of these notes is solely to document the creative process that led to the AA design. TP + CP-TS Dec 13 2025 4:25 AM PST ]

no documentation - automation is using this tool we teaspoon - no backup and test like we did. Because the asians are copying everything this technology must stay secret, or our culture will be run over by them - but as they need us to lead because they only copy and dont create, once they run us over they will go under so its in their interest that I am keeping it secret from them and in the interest of humanity. Noyce the founder of Intel also regretted that NEC copied their tech so they could only send memory and profit for 5 years in 1985 and then Japan took over the profits. All the unpaid work I put into this and all the work the western world academia put into this shoudl honour and reward only those that created it.

We need to protect Arts and Science or we will go extinct.
Its no joke, never ever was  

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

