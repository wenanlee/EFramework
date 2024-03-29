// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------
// Autogenerated with Coco/R, dont change it manually.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using EFramework.Math;

namespace EFramework.Scripting.Internal {


class Parser {
	public const int _EOF = 0;
	public const int _IDENT = 1;
	public const int _NUMBER = 2;
	public const int _STRING = 3;
	public const int maxT = 31;

	const bool _T = true;
	const bool _x = false;
	const int _minErrDist = 2;

	Scanner _scanner;
	public readonly Vars Vars;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = _minErrDist;

public readonly List<ScriptVar> CallParams = new List<ScriptVar>(8);
	public int CallParamsOffset;
	public ScriptVar RetVal;
	public bool ShowLineInfo = true;

	readonly List<string> _paramList = new List<string>(8);
	bool _isParsing;
	bool _isReturned;

	public void Reset () {
        Vars.Reset ();
        _paramList.Clear();
        CallParams.Clear();
        CallParamsOffset = 0;
        _isReturned = false;
        _isParsing = false;
    }

	public string CallFunction() {
		la = Scanner.EmptyToken;
		_isReturned = false;
		RetVal = new ScriptVar();
		try {
			Get();
			Block();
		} catch (Exception ex) {
			return ex.Message;
		}

		return null;
	}
	bool NotBrace() {
		return _scanner.Peek().val != "(";
	}
//----------------------------------------------------------------------------------------------------------------------


	public Parser(ScriptVm vm, Scanner scanner) {
		_scanner = scanner;
		Vars = new Vars(vm);
	}

	void SynErr (int n) {
		if (errDist >= _minErrDist) {
			Errors.SynErr(ShowLineInfo, la.line, la.col, n);
		}
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= _minErrDist) {
			Errors.SemErr(ShowLineInfo, t.line, t.col, msg);
		}
		errDist = 0;
	}
	
	void Get () {
		while (true) {
			t = la;
			la = _scanner.Scan();
			if (la.kind <= maxT) {
				errDist++;
				break;
			}

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) {
			Get();
		} else {
			SynErr(n);
		}
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) {
			Get();
		}
		else {
			SynErr(n);
			while (!StartOf(follow)) {
				Get();
			}
		}
	}

	bool WeakSeparator(int n, int syFol, int repFol) {
		var kind = la.kind;
		if (kind == n) {
			Get();
			return true;
		} else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}
	
	void ScriptVM() {
		_isParsing = true; 
		while (la.kind == 4) {
			Function();
		}
		_isParsing = false; 
	}

	void Function() {
		_paramList.Clear(); 
		Expect(4);
		Expect(1);
		var funcName = t.val; 
		Expect(5);
		if (la.kind == 1) {
			Get();
			_paramList.Add(t.val);  
			while (la.kind == 6) {
				Get();
				Expect(1);
				_paramList.Add(t.val);  
			}
		}
		Expect(7);
		if (Vars.IsFunctionExists(funcName) || Vars.IsHostFunctionExists(funcName)) {
		SemErr(string.Format("Function '{0}' already declared", funcName));
		}
		Vars.RegisterFunction(funcName, _scanner.PC, _paramList);
		
		Block();
	}

	void Block() {
		
		Expect(8);
		Seq();
		if (!_isParsing) {
		if (_isReturned) {
			return;
		}
		}
		
		Expect(9);
	}

	void Seq() {
		
		while (StartOf(1)) {
			if (la.kind == 26) {
				If();
			} else if (la.kind == 28) {
				Switch();
			} else if (la.kind == 10) {
				Get();
				if (StartOf(2)) {
					Expr(out RetVal);
				}
				Expect(11);
				if (!_isParsing) {
				_isReturned = true;
				return;
				}
				
			} else {
				Decl();
			}
		}
	}

	void If() {
		
		Expect(26);
		Expect(5);
		ScriptVar v; 
		Expr(out v);
		var isValid = v.AsNumber != 0f; var isSwitched = false; 
		Expect(7);
		if (!_isParsing) {
		isSwitched = !isValid;
		if (isSwitched) {
			_isParsing = true;
		}
		}
		
		Block();
		if (isSwitched) {
		isSwitched = false;
		_isParsing = false;
		if (_isReturned) {
			return;
		}
		}
		
		if (la.kind == 27) {
			Get();
			if (!_isParsing) {
			isSwitched = isValid;
			if (isSwitched) {
				_isParsing = true;
			}
			}
			
			Block();
			if (isSwitched) {
			isSwitched = false;
			_isParsing = false;
			if (_isReturned) {
				return;
			}
			}
			
		}
	}

	void Switch() {
		
		Expect(28);
		Expect(5);
		ScriptVar v; var caseType = ScriptVarType.Undefined; var caseFound = false; var casePassed = false; var isSwitched = false; 
		Expr(out v);
		if (!_isParsing) {
		caseType = v.Type;
		if (caseType == ScriptVarType.Undefined) {
			SemErr("Expression should be string or number");
		}
		}
		
		Expect(7);
		Expect(8);
		while (la.kind == 29) {
			Get();
			if (la.kind == 2) {
				Get();
				if (!_isParsing && !caseFound) {
				if (caseType != ScriptVarType.Number) {
					SemErr(string.Format("Invalid type in case, {0} required", caseType));
				}
				if (!casePassed && v.AsNumber == t.val.ToFloatUnchecked()) {
					caseFound = true;
					casePassed = true;
				}
				}
				
			} else if (la.kind == 3) {
				Get();
				if (!_isParsing && !caseFound) {
				if (caseType != ScriptVarType.String) {
					SemErr(string.Format("Invalid type in case, {0} required", caseType));
				}
				if (!_isParsing) {
					if (!casePassed && v.AsString == t.val) {
						caseFound = true;
						casePassed = true;
					}
				}
				}
				
			} else SynErr(32);
			Expect(30);
			if (!_isParsing && !caseFound) {
			_isParsing = true;
			isSwitched = true;
			}
			
			Block();
			if (isSwitched) {
			_isParsing = false;
			isSwitched = false;
			}
			caseFound = false;
			
		}
		Expect(9);
	}

	void Expr(out ScriptVar v) {
		ScriptVar b; int op; 
		Expr1(out v);
		while (la.kind == 12 || la.kind == 13) {
			if (la.kind == 12) {
				Get();
				op = 0; 
			} else {
				Get();
				op = 1; 
			}
			Expr1(out b);
			if (!_isParsing) {
			if (!v.IsNumber || !b.IsNumber) {
				SemErr("'<' operator can be applied to numbers only");
			}
			switch (op) {
				case 0:
					v.AsNumber = v.AsNumber != 0f || b.AsNumber != 0f ? 1f : 0f;
					break;
				case 1:
					v.AsNumber = v.AsNumber != 0f && b.AsNumber != 0f ? 1f : 0f;
					break;
			}
			}
			
		}
	}

	void Decl() {
		var isNew = false; var type = ScriptVarType.Undefined; ScriptVar v; var isCalling = false; string name = null; var isAssigned = false; 
		if (la.kind == 24) {
			Get();
			isNew = true; 
		}
		if (NotBrace()) {
			Expect(1);
			name = t.val;
			if (!_isParsing) {
			var isExists = Vars.IsFunctionExists(name) || Vars.IsHostFunctionExists(name);
			if (isExists) {
				SemErr(string.Format("Function '{0}' exists and cant be assigned as variable", name));
			}
			isExists = Vars.IsVarExists(name);
			if (isNew && isExists) {
				SemErr(string.Format("Variable '{0}' already declared", name));
			}
			if (isExists) {
				type = Vars.GetVar(name).Type;
			}
			if (!isNew && !isExists) {
				SemErr(string.Format("Variable '{0}' not declared", name));
			}
			}
			
		} else if (StartOf(2)) {
			if (isNew) { SemErr("Invalid usage of variable declaration"); } 
			Expr(out v);
			isCalling = true; 
		} else SynErr(33);
		if (la.kind == 25) {
			if (isCalling) { SemErr("Only variable can be assigned, not expression"); } 
			Get();
			Expr(out v);
			if (!_isParsing) {
			if (type != ScriptVarType.Undefined && v.Type != type) {
				SemErr(string.Format("Variable '{0}' type cant be changed", name));
			}
			Vars.RegisterVar(name, v);
			} else {
			isAssigned = true;
			}
			
		}
		Expect(11);
		if (_isParsing && isNew && !isCalling && !isAssigned) { SemErr(string.Format("Variable '{0}' should be initialized", name)); } 
	}

	void Expr1(out ScriptVar v) {
		ScriptVar b; int op; 
		Expr2(out v);
		while (la.kind == 14 || la.kind == 15) {
			if (la.kind == 14) {
				Get();
				op = 0; 
			} else {
				Get();
				op = 1; 
			}
			Expr2(out b);
			if (!_isParsing) {
			switch (op) {
				case 0:
					if (v.Type != b.Type) {
						v.AsNumber = 0f;
					} else {
						v.AsNumber = v.IsNumber ? (v.AsNumber == b.AsNumber ? 1f : 0f) : (v.AsString == b.AsString ? 1f : 0f);
					}
					break;
				case 1:
					if (v.Type != b.Type) {
						v.AsNumber = 1f;
					} else {
						v.AsNumber = v.IsNumber ? (v.AsNumber != b.AsNumber ? 1f : 0f) : (v.AsString != b.AsString ? 1f : 0f);
					}
					break;
			}
			}
			
		}
	}

	void Expr2(out ScriptVar v) {
		ScriptVar b; int mode; 
		Expr3(out v);
		while (StartOf(3)) {
			if (la.kind == 16) {
				Get();
				mode = 0; 
			} else if (la.kind == 17) {
				Get();
				mode = 1; 
			} else if (la.kind == 18) {
				Get();
				mode = 2; 
			} else {
				Get();
				mode = 3; 
			}
			Expr3(out b);
			if (!_isParsing) {
			if (!v.IsNumber || !b.IsNumber) {
				SemErr("'<' operator can be applied to numbers only");
			}
			switch (mode) {
				case 0:				
					v.AsNumber = v.AsNumber < b.AsNumber ? 1f : 0f;
					break;
				case 1:
					v.AsNumber = v.AsNumber > b.AsNumber ? 1f : 0f;
					break;
				case 2:
					v.AsNumber = v.AsNumber <= b.AsNumber ? 1f : 0f;
					break;
				case 3:
					v.AsNumber = v.AsNumber >= b.AsNumber ? 1f : 0f;
					break;
			}
			}
			
		}
	}

	void Expr3(out ScriptVar v) {
		ScriptVar b; bool isSub; 
		Expr4(out v);
		while (la.kind == 20 || la.kind == 21) {
			if (la.kind == 20) {
				Get();
				isSub = false; 
			} else {
				Get();
				isSub = true; 
			}
			Expr4(out b);
			if (!_isParsing) {
			if (v.IsString || b.IsString) {
				if (isSub) {
					SemErr("Operator '-' cant be applied to strings");
				} else {
					v.AsString = v.AsString + b.AsString;
				}
			} else {
				if (v.IsNumber || b.IsNumber) {
					v.AsNumber = v.AsNumber + (isSub ? -b.AsNumber : b.AsNumber);
				}
			}
			}
			
		}
	}

	void Expr4(out ScriptVar v) {
		ScriptVar b; 
		Expr5(out v);
		while (la.kind == 22 || la.kind == 23) {
			bool isDiv; 
			if (la.kind == 22) {
				Get();
				isDiv = false; 
			} else {
				Get();
				isDiv = true; 
			}
			Expr5(out b);
			if (!_isParsing) {
			if (!v.IsNumber || !b.IsNumber) {
				SemErr(string.Format("Operator '{0}' cant be applied to numbers only", isDiv ? '/' : '*'));
			}
			v.AsNumber = isDiv ? (b.AsNumber == 0f ? 0f : v.AsNumber / b.AsNumber) : v.AsNumber * b.AsNumber;
			}
			
		}
	}

	void Expr5(out ScriptVar v) {
		v = new ScriptVar(); var isNegative = false; 
		if (la.kind == 21) {
			Get();
			isNegative = true; 
		}
		if (la.kind == 5) {
			Get();
			Expr(out v);
			Expect(7);
		} else if (la.kind == 1 || la.kind == 2 || la.kind == 3) {
			Term(out v);
		} else SynErr(34);
		if (!_isParsing) {
		if (isNegative) {
			if (v.IsNumber) {
					v.AsNumber = -v.AsNumber;
			} else {
				SemErr("Minus can be applied only on numbers");
			}
		}
		}
		
	}

	void Term(out ScriptVar v) {
		v = new ScriptVar(); var callOffset = int.MaxValue; var isCall = false; 
		if (la.kind == 1) {
			Get();
			var identName = t.val; 
			if (la.kind == 5) {
				Get();
				ScriptVar p; isCall = true; if (!_isParsing) { callOffset = CallParams.Count; } 
				if (StartOf(2)) {
					Expr(out p);
					if (!_isParsing) { CallParams.Add(p); } 
					while (la.kind == 6) {
						Get();
						Expr(out p);
						if (!_isParsing) { CallParams.Add(p); } 
					}
				}
				Expect(7);
			}
			if (isCall) {
			if (!Vars.IsHostFunctionExists(identName)) {
				SemErr(string.Format("Cant find host function with name '{0}'", identName));
			}
			}
			if (!_isParsing) {
			if (isCall) {
				CallParamsOffset = callOffset;
				v = Vars.CallHostFunction(identName);
				while (CallParams.Count > callOffset) {
					CallParams.RemoveAt(CallParams.Count - 1);
				}
			} else {
				if (!Vars.IsVarExists(t.val)) {
					SemErr(string.Format("Variable '{0}' not found", identName));
				}
				v = Vars.GetVar(identName);
			}
			}
			
		} else if (la.kind == 2) {
			Get();
			if (!_isParsing) {
			v.AsNumber = t.val.ToFloatUnchecked();
			}
			
		} else if (la.kind == 3) {
			Get();
			if (!_isParsing) {
			v.AsString = t.val;
			}
			
		} else SynErr(35);
	}


	public string Parse() {
		la = Scanner.EmptyToken;
		try {
			Get();
		ScriptVM();
		Expect(0);

		} catch (Exception ex) {
			return ex.Message;
		}
		return null;
	}
	
	static readonly bool[,] set = {
		{_T,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x},
		{_x,_T,_T,_T, _x,_T,_x,_x, _x,_x,_T,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _T,_x,_T,_x, _T,_x,_x,_x, _x},
		{_x,_T,_T,_T, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_T,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x},
		{_x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _T,_T,_T,_T, _x,_x,_x,_x, _x,_x,_x,_x, _x,_x,_x,_x, _x}

	};
}

static class Errors {
	const string ErrFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

	public static void SynErr (bool showLineInfo, int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "IDENT expected"; break;
			case 2: s = "NUMBER expected"; break;
			case 3: s = "STRING expected"; break;
			case 4: s = "\"function\" expected"; break;
			case 5: s = "\"(\" expected"; break;
			case 6: s = "\",\" expected"; break;
			case 7: s = "\")\" expected"; break;
			case 8: s = "\"{\" expected"; break;
			case 9: s = "\"}\" expected"; break;
			case 10: s = "\"return\" expected"; break;
			case 11: s = "\";\" expected"; break;
			case 12: s = "\"||\" expected"; break;
			case 13: s = "\"&&\" expected"; break;
			case 14: s = "\"==\" expected"; break;
			case 15: s = "\"!=\" expected"; break;
			case 16: s = "\"<\" expected"; break;
			case 17: s = "\">\" expected"; break;
			case 18: s = "\"<=\" expected"; break;
			case 19: s = "\">=\" expected"; break;
			case 20: s = "\"+\" expected"; break;
			case 21: s = "\"-\" expected"; break;
			case 22: s = "\"*\" expected"; break;
			case 23: s = "\"/\" expected"; break;
			case 24: s = "\"var\" expected"; break;
			case 25: s = "\"=\" expected"; break;
			case 26: s = "\"if\" expected"; break;
			case 27: s = "\"else\" expected"; break;
			case 28: s = "\"switch\" expected"; break;
			case 29: s = "\"case\" expected"; break;
			case 30: s = "\":\" expected"; break;
			case 31: s = "??? expected"; break;
			case 32: s = "invalid Switch"; break;
			case 33: s = "invalid Decl"; break;
			case 34: s = "invalid Expr5"; break;
			case 35: s = "invalid Term"; break;

			default: s = "error " + n; break;
		}
		SemErr(showLineInfo, line, col, s);
	}

	public static void SemErr (bool showLineInfo, int line, int col, string msg) {
		throw new Exception ( showLineInfo ? string.Format(ErrFormat, line, col, msg) : msg);
	}
}}