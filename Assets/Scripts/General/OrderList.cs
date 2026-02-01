using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class OrderList : MonoBehaviour
{
    public List<OrderText> orders = new List<OrderText>();

    private void Awake()
    {
        Dependencies.Instance.RegisterDependency<OrderList>(this);
    }

}
