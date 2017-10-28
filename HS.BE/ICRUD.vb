﻿Imports System.Collections.Generic

''' <summary>
''' Interfaz que define los metodos que todas las clases que expongan mecanismos de
''' Alta, Baja, Modificacion y Consulta deben implementar (Create Read Update Delete).
''' </summary>
''' <typeparam name="T"></typeparam>
''' <remarks></remarks>
Public Interface ICRUD(Of T)
    ''' <summary>
    ''' Agrega un objeto del tipo T.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Alta(ByRef value As T) As Boolean

    ''' <summary>
    ''' Elimina un objeto del tipo T ya existente.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Baja(ByRef value As T) As Boolean

    ''' <summary>
    ''' Modifica un objeto del tipo T ya existente.
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Modificacion(ByRef value As T) As Boolean

    ''' <summary>
    ''' Retorna el primer objeto del tipo T que coincida con el filtro especificado.
    ''' </summary>
    ''' <param name="filtro"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Consulta(ByRef filtro As T) As T

    ''' <summary>
    ''' Retorna todos los objetos del tipo T que coincidan con los valores
    ''' especificados en el rango desde-hasta.
    ''' </summary>
    ''' <param name="filtroDesde"></param>
    ''' <param name="filtroHasta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ConsultaRango(ByRef filtroDesde As T, ByRef filtroHasta As T) As List(Of T)

End Interface

