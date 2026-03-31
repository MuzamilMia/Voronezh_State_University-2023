
% Защита от деления на ноль
long_div(_, 0, _, _) :-
    throw(error(divide_by_zero, 'Деление на ноль невозможно.')).

long_div(Dividend, Divisor, Quotient, Remainder) :-
    % Если делимое равно 0 — сразу ответ
    Dividend =:= 0,
    Quotient = 0,
    Remainder = 0, !.

long_div(Dividend, Divisor, Quotient, Remainder) :-
    % Обработка знаков
    (Dividend >= 0 -> SignD = 1 ; SignD = -1),
    (Divisor  >= 0 -> SignV = 1 ; SignV = -1),

    AbsD is abs(Dividend),
    AbsV is abs(Divisor),

    number_digits(AbsD, Digits),
    divide_digits(Digits, AbsV, 0, [], QDigits, RAbs),
    (QDigits = [] -> QAbs = 0 ; digits_number(QDigits, QAbs)),

    Quotient is QAbs * SignD * SignV,
    Remainder is RAbs * SignD.

divide_digits([], _, R, Q, Q, R).
divide_digits([D|Ds], Divisor, Current, QAcc, Q, R) :-
    New is Current * 10 + D,
    Digit is New // Divisor,
    Rem is New mod Divisor,
    divide_digits(Ds, Divisor, Rem, [Digit|QAcc], Q, R).

number_digits(N, Digits) :-
    number_chars(N, Cs),
    maplist(atom_number, Cs, Digits).

digits_number(Ds, N) :-
    reverse(Ds, R),
    digits_number_(R, 0, N).

digits_number_([], Acc, Acc).
digits_number_([D|Ds], Acc, N) :-
    NewAcc is Acc * 10 + D,
    digits_number_(Ds, NewAcc, N).
