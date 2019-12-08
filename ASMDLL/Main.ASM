.DATA
result  qword    ?
tmp qword ?

.CODE
; RCX - PIERWSZY PRZEKAZANY INT
; RDX - DRUGI PRZEKAZYWANY INT
; R8 - Trzeci Int

testFunctionASM PROC

MOVD MM1, RCX
MOVD MM2, RDX

;koniec funkcji
                ret
testFunctionASM ENDP
END