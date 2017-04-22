using UnityEngine;
using System.Collections;

public class SkillWindowC : MonoBehaviour {
	
	public GameObject player;
	public GameObject database;
	
	public int[] skill = new int[3];
	public int[] skillListSlot = new int[9];
	
	private bool  menu = false;
	private bool  shortcutPage = true;
	private bool  skillListPage = false;
	private int skillSelect = 0;
	
	void  Start (){
		if(!player){
			player = this.gameObject;
		}
		//AssignAllSkill();
		
	}
	
	void  Update (){
		if (Input.GetKeyDown("k")) {
			OnOffMenu();
		}
		
	}
	
	void  OnOffMenu (){
		//Freeze Time Scale to 0 if Window is Showing
		if(!menu && Time.timeScale != 0.0f){
			menu = true;
			skillListPage = false;
			shortcutPage = true;
			Time.timeScale = 0.0f;
			Screen.lockCursor = false;
		}else if(menu){
			menu = false;
			Time.timeScale = 1.0f;
			Screen.lockCursor = true;
		}
	}
	
	void  OnGUI (){
		SkillDataC dataItem = database.GetComponent<SkillDataC>();
		if(menu && shortcutPage){
			GUI.Box ( new Rect(180,160,360,185), "Skill");
			//Close Window Button
			if (GUI.Button ( new Rect(490,162,30,30), "X")) {
				OnOffMenu();
			}
			
			//Skill Shortcut
			if (GUI.Button ( new Rect(210,205,80,80), dataItem.skill[skill[0]].icon)) {
				skillSelect = 0;
				skillListPage = true;
				shortcutPage = false;
			}
			GUI.Label ( new Rect(250, 305, 20, 20), "1");
			if (GUI.Button ( new Rect(310,205,80,80), dataItem.skill[skill[1]].icon)) {
				skillSelect = 1;
				skillListPage = true;
				shortcutPage = false;
			}
			GUI.Label ( new Rect(350, 305, 20, 20), "2");
			if (GUI.Button ( new Rect(410,205,80,80), dataItem.skill[skill[2]].icon)) {
				skillSelect = 2;
				skillListPage = true;
				shortcutPage = false;
			}
			GUI.Label ( new Rect(450, 305, 20, 20), "3");
		}
		//---------------Skill List----------------------------
		if(menu && skillListPage){
			GUI.Box ( new Rect(160,160,380,385), "Skill");
			//Close Window Button
			if (GUI.Button ( new Rect(500,162,30,30), "X")) {
				OnOffMenu();
			}
			if (GUI.Button ( new Rect(210,215,80,80), dataItem.skill[skillListSlot[0]].icon)) {
				AssignSkill(skillSelect , 0);
				shortcutPage = true;
				skillListPage = false;		
			}
			if (GUI.Button ( new Rect(310,215,80,80), dataItem.skill[skillListSlot[1]].icon)) {
				AssignSkill(skillSelect , 1);
				shortcutPage = true;
				skillListPage = false;		
			}
			if (GUI.Button ( new Rect(410,215,80,80), dataItem.skill[skillListSlot[2]].icon)) {
				AssignSkill(skillSelect , 2);
				shortcutPage = true;
				skillListPage = false;		
			}
			//-----------------
			if (GUI.Button ( new Rect(210,315,80,80), dataItem.skill[skillListSlot[3]].icon)) {
				AssignSkill(skillSelect , 3);
				shortcutPage = true;
				skillListPage = false;		
			}
			if (GUI.Button ( new Rect(310,315,80,80), dataItem.skill[skillListSlot[4]].icon)) {
				AssignSkill(skillSelect , 4);
				shortcutPage = true;
				skillListPage = false;		
			}
			if (GUI.Button ( new Rect(410,315,80,80), dataItem.skill[skillListSlot[5]].icon)) {
				AssignSkill(skillSelect , 5);
				shortcutPage = true;
				skillListPage = false;		
			}
			//-----------------
			if (GUI.Button ( new Rect(210,415,80,80), dataItem.skill[skillListSlot[6]].icon)) {
				AssignSkill(skillSelect , 6);
				shortcutPage = true;
				skillListPage = false;		
			}
			if (GUI.Button ( new Rect(310,415,80,80), dataItem.skill[skillListSlot[7]].icon)) {
				AssignSkill(skillSelect , 7);
				shortcutPage = true;
				skillListPage = false;		
			}
			if (GUI.Button ( new Rect(410,415,80,80), dataItem.skill[skillListSlot[8]].icon)) {
				AssignSkill(skillSelect , 8);
				shortcutPage = true;
				skillListPage = false;		
			}
		}
		
	}
	
	public void  AssignSkill ( int id  ,   int sk  ){
		SkillDataC dataSkill = database.GetComponent<SkillDataC>();
		player.GetComponent<AttackTriggerC>().manaCost[id] = dataSkill.skill[skillListSlot[sk]].manaCost;
		player.GetComponent<AttackTriggerC>().skillPrefab[id] = dataSkill.skill[skillListSlot[sk]].skillPrefab;
		player.GetComponent<AttackTriggerC>().skillAnimation[id] = dataSkill.skill[skillListSlot[sk]].skillAnimation;
		
		if(dataSkill.skill[skillListSlot[sk]].skillAnimation){
			player.GetComponent<AttackTriggerC>().mainModel.GetComponent<Animation>()[dataSkill.skill[skillListSlot[sk]].skillAnimation.name].layer = 16;
		}
		
		player.GetComponent<AttackTriggerC>().skillIcon[id] = dataSkill.skill[skillListSlot[sk]].icon;
		skill[id] = skillListSlot[sk];
		print(sk);
		
	}
	
	public void  AssignAllSkill (){
		if(!player){
			player = this.gameObject;
		}
		int n = 0;
		SkillDataC dataSkill = database.GetComponent<SkillDataC>();
		while(n <= 2){
			player.GetComponent<AttackTriggerC>().manaCost[n] = dataSkill.skill[skill[n]].manaCost;
			player.GetComponent<AttackTriggerC>().skillPrefab[n] = dataSkill.skill[skill[n]].skillPrefab;
			player.GetComponent<AttackTriggerC>().skillAnimation[n] = dataSkill.skill[skill[n]].skillAnimation;
			
			if(dataSkill.skill[skill[n]].skillAnimation){
				player.GetComponent<AttackTriggerC>().mainModel.GetComponent<Animation>()[dataSkill.skill[skill[n]].skillAnimation.name].layer = 16;
			}
			
			player.GetComponent<AttackTriggerC>().skillIcon[n] = dataSkill.skill[skill[n]].icon;
			n++;
		}
		
	}
}