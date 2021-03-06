﻿Imports DevExpress.DashboardCommon
Imports DevExpress.XtraEditors
Imports System
Imports System.Windows.Forms
Imports System.Xml.Linq

Namespace WinFormsViewerSaveAndApplyDashboardState
	Partial Public Class ViewerForm1
		Inherits XtraForm

		Private dState As New DashboardState()
		Private Const path As String = "..\..\Dashboards\dashboardWithSavedState.xml"
		Public Sub New()
			InitializeComponent()
			AddHandler dashboardViewer.DashboardLoaded, AddressOf dashboardViewer_DashboardLoaded
			AddHandler dashboardViewer.SetInitialDashboardState, AddressOf dashboardViewer_SetInitialDashboardState
			dashboardViewer.DashboardSource = path
		End Sub

		Private Sub dashboardViewer_DashboardLoaded(ByVal sender As Object, ByVal e As DevExpress.DashboardWin.DashboardLoadedEventArgs)
			Dim data As XElement = e.Dashboard.UserData
				If data IsNot Nothing Then
					If data.Element("DashboardState") IsNot Nothing Then
						Dim dStateDocument As XDocument = XDocument.Parse(data.Element("DashboardState").Value)
						dState.LoadFromXml(XDocument.Parse(data.Element("DashboardState").Value))
					End If
				End If
		End Sub

		Private Sub dashboardViewer_SetInitialDashboardState(ByVal sender As Object, ByVal e As DevExpress.DashboardWin.SetInitialDashboardStateEventArgs)
			e.InitialState = dState
		End Sub

		Private Sub ViewerForm1_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
			dState = dashboardViewer.GetDashboardState()
			Dim userData As New XElement("Root", New XElement("DateModified",Date.Now), New XElement("DashboardState",dState.SaveToXml().ToString(SaveOptions.DisableFormatting)))
			dashboardViewer.Dashboard.UserData = userData
			dashboardViewer.Dashboard.SaveToXml(path)
		End Sub
	End Class
End Namespace
