using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 후에 어드레서블로 변경 예정
/// </summary>
public class PopUpList : MonoBehaviour
{
    [SerializeField] private GameObject alertPopUp;
    [SerializeField] private GameObject confirmPopUp;
    
    public GameObject AlertPopUp => alertPopUp;
    public GameObject ConfirmPopUp => confirmPopUp;
}
