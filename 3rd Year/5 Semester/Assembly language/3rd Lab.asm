                                      .model small
.stack 100h

.data
    ; User interface messages
    msg_prompt      db 10, 13, 'Enter a sentence: $'
    msg_letter      db 10, 13, 'Enter a letter: $'
    msg_input_echo  db 10, 13, 'Your sentence: $'
    msg_result      db 10, 13, 'Words starting/ending with letter: $'
    msg_newline     db 10, 13, '$'
    
    ; Data buffers and variables
    sentence_buffer db 100, 0, 100 dup(0)  ; Buffer format: max_len, actual_len, string_data
    search_letter   db ?                   ; Letter to search for
    word_count      dw 0                   ; Counter for words found

.code
main proc
    ; Initialize data segment
    mov ax, @data
    mov ds, ax
    mov es, ax          ; ES also points to data segment for string operations
    
    ; Display prompt for sentence input
    mov ah, 09h
    lea dx, msg_prompt
    int 21h
    
    ; Read sentence from keyboard (DOS buffered input function 0Ah)
    mov ah, 0Ah
    lea dx, sentence_buffer
    int 21h
    
    ; Display echo message
    mov ah, 09h
    lea dx, msg_input_echo
    int 21h
    
    ; Prepare string for display: replace CR with '$'
    mov cl, sentence_buffer + 1    ; Get actual string length (byte at offset 1)
    xor ch, ch                     ; Clear upper byte of CX
    lea si, sentence_buffer + 2    ; SI points to start of string data
    add si, cx                     ; Move SI to end of string (where CR is)
    mov byte ptr [si], '$'         ; Replace carriage return with '$' for display
    
    ; Display the input sentence
    mov ah, 09h
    lea dx, sentence_buffer + 2
    int 21h
    
    ; Print newline
    mov ah, 09h
    lea dx, msg_newline
    int 21h
    
    ; Prompt for search letter
    mov ah, 09h
    lea dx, msg_letter
    int 21h
    
    ; Read single character (search letter)
    mov ah, 01h
    int 21h
    mov search_letter, al          ; Store the letter
    
    ; Print newline
    mov ah, 09h
    lea dx, msg_newline
    int 21h
    
    ; PUSH PARAMETERS TO STACK (reverse order)
    ; Parameter 3: Sentence length
    mov al, sentence_buffer + 1    ; Get actual length
    xor ah, ah                     ; Convert to 16-bit
    push ax                        ; Push length
    
    ; Parameter 2: Search letter  
    mov al, search_letter          ; Get search letter
    xor ah, ah                     ; Convert to 16-bit
    push ax                        ; Push letter
    
    ; Parameter 1: Sentence address
    lea ax, sentence_buffer + 2    ; Get address of string data
    push ax                        ; Push address
    
    ; Call the word counting procedure
    call count_words_proc
    
    ; Get result and clean up stack
    mov word_count, ax             ; Store returned word count
    add sp, 6                      ; Clean up 3 parameters (2 bytes each)
    
    ; Display result message
    mov ah, 09h
    lea dx, msg_result
    int 21h
    
    ; Display the count as decimal number
    mov ax, word_count
    call print_number
    
    ; Wait for key press before exit
    mov ah, 09h
    lea dx, msg_newline
    int 21h
    mov ah, 01h
    int 21h
    
    ; Exit to DOS
    mov ah, 4Ch
    int 21h
main endp

; =============================================
; Procedure: count_words_proc
; Counts words that start OR end with specified letter
; Parameters passed via stack:
;   [BP+4]  = Address of sentence string
;   [BP+6]  = Letter to search for
;   [BP+8]  = Length of sentence
; Returns: AX = Number of matching words
; Uses string commands: repe scasb, repnz scasb, scasb
; =============================================
count_words_proc proc
    ; Prologue: save registers and set up stack frame
    push bp
    mov bp, sp
    push bx
    push cx
    push si
    push di
    
    ; Get parameters from stack
    mov si, [bp+4]      ; SI = sentence address
    mov al, byte ptr [bp+6]  ; AL = search letter (byte parameter)
    mov cx, [bp+8]      ; CX = sentence length
    xor bx, bx          ; BX = word counter (initialize to 0)
    cld                 ; Clear direction flag (process forward)
    
    ; Check for empty string
    cmp cx, 0
    je proc_done
    
    mov di, si          ; DI points to sentence for string operations
    
main_loop:
    ; Check if we've processed entire string
    cmp cx, 0
    je proc_done
    
    ; Skip leading spaces using repe scasb
    ; repe = repeat while equal (ZF=1) and CX?0
    mov al, ' '         ; Search for spaces to skip
    repe scasb          ; Skip all consecutive spaces
    dec di              ; Back up to first non-space character
    inc cx              ; Adjust counter (because scasb decremented it)
    
    ; Check for end of string marker
    cmp byte ptr [di], '$'
    je proc_done
    
    ; Save current position for later
    push di
    push cx
    
    ; Check if word starts with search letter
    mov al, search_letter
    scasb               ; Compare AL with [DI], then increment DI
    je word_starts_with_letter  ; Jump if first character matches
    
    ; Word doesn't start with letter - check if it ends with letter
    dec di              ; Move back to start of word
    
    ; Find end of current word using repnz scasb
    ; repnz = repeat while not equal (ZF=0) and CX?0
    mov cx, [bp+8]      ; Reload original length from stack
    sub cx, di          ; Calculate remaining characters
    add cx, si          ; Adjust calculation
    mov ax, cx          ; Save to AX
    mov cx, ax          ; Restore to CX
    
    mov al, ' '         ; Look for space (word delimiter)
    repnz scasb         ; Scan until space found or CX=0
    jz found_space      ; Jump if space was found (ZF=1)
    
    ; No space found - we're at end of sentence
    dec di              ; Point to last character
    jmp check_end_char
    
found_space:
    ; Space found - adjust pointers to last character of word
    dec di              ; Back up from space
    dec di              ; Point to last character of word
    
check_end_char:
    ; Check if word ends with search letter
    mov al, search_letter
    cmp al, [di]        ; Compare with last character
    jne next_word_prep  ; Jump if no match
    inc bx              ; Increment counter - word ends with letter
    
next_word_prep:
    ; Restore position and continue
    pop cx
    pop di
    jmp skip_to_next_word

word_starts_with_letter:
    ; Word starts with search letter
    inc bx              ; Increment counter
    pop cx              ; Restore saved CX
    pop di              ; Restore saved DI position
    
    ; Skip to next word using repnz scasb
skip_to_next_word:
    mov al, ' '         ; Search for next space
    repnz scasb         ; Scan until space or end of string
    
    jmp main_loop       ; Process next word
    
proc_done:
    ; Epilogue: return result and restore registers
    mov ax, bx          ; Return word count in AX
    
    pop di
    pop si
    pop cx
    pop bx
    pop bp
    ret
count_words_proc endp

; =============================================
; Procedure: print_number
; Prints a 16-bit unsigned integer in decimal
; Input: AX = number to print
; Uses stack to reverse digits
; =============================================
print_number proc
    push ax
    push bx
    push cx
    push dx
    
    ; Handle zero as special case
    cmp ax, 0
    jne convert
    mov dl, '0'         ; Print '0' directly
    mov ah, 02h
    int 21h
    jmp print_done
    
convert:
    ; Convert number to decimal digits
    mov cx, 0           ; Digit counter
    mov bx, 10          ; Divisor for base-10
    
divide_loop:
    xor dx, dx          ; Clear DX for division (DX:AX / BX)
    div bx              ; AX = quotient, DX = remainder (digit)
    add dl, '0'         ; Convert digit to ASCII
    push dx             ; Save digit on stack (reverse order)
    inc cx              ; Count digits
    test ax, ax         ; Check if quotient is zero
    jnz divide_loop     ; Continue if not zero
    
    ; Print digits from stack (now in correct order)
print_loop:
    pop dx              ; Get digit from stack
    mov ah, 02h         ; DOS print character function
    int 21h
    loop print_loop     ; Repeat for all digits
    
print_done:
    ; Restore registers
    pop dx
    pop cx
    pop bx
    pop ax
    ret
print_number endp

end main