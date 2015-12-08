#pragma strict
var prefabName:GUIText;
var particlePrefab : GameObject[];
var particleNum : int = 0;

private var effectPrefab : GameObject;

function Update () {
	var mouseX = Input.mousePosition.x; 
	var mouseY = Input.mousePosition.y; 
	var ray = GetComponent.<Camera>().main.ScreenPointToRay (Vector3(mouseX,mouseY,0));
	var hit : RaycastHit;

	if (Physics.Raycast (ray, hit, 200)) {}
	if ( Input.GetMouseButtonDown(0) ){ 
		if(particleNum >= 0 && particleNum <= 3){
			effectPrefab = Instantiate(particlePrefab[particleNum],
				new Vector3(hit.point.x,hit.point.y + 0.2,hit.point.z), Quaternion.Euler(0,0,0));
			}
		if(particleNum >= 4 && particleNum <= 10){
			effectPrefab = Instantiate(particlePrefab[particleNum],
				new Vector3(hit.point.x,hit.point.y + 1,hit.point.z), Quaternion.Euler(0,0,0));
			}
	}
	if (Input.GetKeyDown(KeyCode.LeftArrow)){
		particleNum -= 1;
		if( particleNum < 0) {
			particleNum = particlePrefab.length-1;
			}		
		}
	if (Input.GetKeyDown(KeyCode.RightArrow)){
		particleNum += 1;
		if(particleNum >(particlePrefab.length - 1)) {
			particleNum = 0;
			}
		}
		
	prefabName.text= particlePrefab[particleNum].name;
	

}