#pragma strict
public var runMaxAnimationSpeed : float = 1.0;
public var backMaxAnimationSpeed : float = 1.0;
public var sprintAnimationSpeed : float = 1.5;

private var player : GameObject;
private var mainModel : GameObject;

//var idle : String = "idle";
var idle : AnimationClip;
var run : AnimationClip;
var right : AnimationClip;
var left : AnimationClip;
var back : AnimationClip;
var jump : AnimationClip;
var hurt : AnimationClip;

	/*mainModel.animation[run.name].speed = runMaxAnimationSpeed;
	mainModel.animation[right.name].speed = runMaxAnimationSpeed;
	mainModel.animation[left.name].speed = runMaxAnimationSpeed;
	mainModel.animation[back.name].speed = backMaxAnimationSpeed;*/

function Start () {
	if(!player){
		player = this.gameObject;
	}
	mainModel = GetComponent(AttackTrigger).mainModel;
	if(!mainModel){
		mainModel = this.gameObject;
	}
	
	mainModel.GetComponent.<Animation>()[run.name].speed = runMaxAnimationSpeed;
	mainModel.GetComponent.<Animation>()[right.name].speed = runMaxAnimationSpeed;
	mainModel.GetComponent.<Animation>()[left.name].speed = runMaxAnimationSpeed;
	mainModel.GetComponent.<Animation>()[back.name].speed = backMaxAnimationSpeed;
	
	mainModel.GetComponent.<Animation>()[jump.name].wrapMode  = WrapMode.ClampForever;
	
	if(hurt){
		mainModel.GetComponent.<Animation>()[hurt.name].layer = 5;
	}
	
}

function Update () {
    var controller : CharacterController = player.GetComponent(CharacterController);
    if ((controller.collisionFlags & CollisionFlags.Below) != 0){
        if (Input.GetAxis("Horizontal") > 0.1)
      mainModel.GetComponent.<Animation>().CrossFade(right.name);
   else if (Input.GetAxis("Horizontal") < -0.1)
      mainModel.GetComponent.<Animation>().CrossFade(left.name);
   else if (Input.GetAxis("Vertical") > 0.1)
      mainModel.GetComponent.<Animation>().CrossFade(run.name);
   else if (Input.GetAxis("Vertical") < -0.1)
      mainModel.GetComponent.<Animation>().CrossFade(back.name);
   else
      mainModel.GetComponent.<Animation>().CrossFade(idle.name);
	}else{
		mainModel.GetComponent.<Animation>().CrossFade(jump.name);
	}
}

function AnimationSpeedSet(){
		mainModel = GetComponent(AttackTrigger).mainModel;
		if(!mainModel){
			mainModel = this.gameObject;
		}
		mainModel.GetComponent.<Animation>()[run.name].speed = runMaxAnimationSpeed;
		mainModel.GetComponent.<Animation>()[right.name].speed = runMaxAnimationSpeed;
		mainModel.GetComponent.<Animation>()[left.name].speed = runMaxAnimationSpeed;
		mainModel.GetComponent.<Animation>()[back.name].speed = backMaxAnimationSpeed;
}