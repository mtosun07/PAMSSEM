rm(list=ls())
source("PAMSSEMCalculator.R")
cat("\f")



#Kitaptaki problemin çözümü:

InitializePAMSSEM(
  c("A1", "A2", "A3"),
  TRUE,
  c("C1", "C2", "C3"),
  c(1/3, 1/3, 1/3),
  c(5, 15, 1),
  c(12, 25, 2),
  c(18, 32, 3),
  NA,
  c(FALSE, TRUE, TRUE),
  c(80, 65, 83, 90, 58, 60, 5, 2, 7))

SolvePAMSSEM_Stage1()

SolvePAMSSEM_Stage2()

SolvePAMSSEM_Stage3()

SolvePAMSSEM_Stage4()

SolvePAMSSEM_Stage5()

SolvePAMSSEM_Stage6()

SolvePAMSSEM_Finalize1()

SolvePAMSSEM_Finalize2()